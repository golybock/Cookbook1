﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class IngredientRepository : MainDbClass, IIngredientRepository
{
    public async Task<Ingredient> GetIngredientAsync(int id)
    {
        connection.Open();
        try
        {
            Ingredient ingredient = new Ingredient();
            string query = $"select * from ingredient where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                ingredient.MeasureId = reader.GetInt32(reader.GetOrdinal("measure_id"));
                ingredient.Name = reader.GetString(reader.GetOrdinal("name"));
                var imagePath = reader.GetValue(reader.GetOrdinal("image_path"));
                ingredient.ImagePath = imagePath == DBNull.Value ? null : imagePath.ToString();
            }
            
            return ingredient;
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        connection.Open();
        try
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            string query = $"select * from ingredient";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                Ingredient ingredient = new Ingredient();
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                ingredient.MeasureId = reader.GetInt32(reader.GetOrdinal("measure_id"));
                ingredient.Name = reader.GetString(reader.GetOrdinal("name"));
                var imagePath = reader.GetValue(reader.GetOrdinal("image_path"));
                ingredient.ImagePath = imagePath == DBNull.Value ? null : imagePath.ToString();
                ingredients.Add(ingredient);
            }
            
            return ingredients;
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddIngredientAsync(Ingredient ingredient)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into ingredient(measure_id, name, image_path)" +
                           $" values ($1, $2, $3) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = ingredient.MeasureId },
                    new() { Value = ingredient.Name },
                    new() { Value = ingredient.ImagePath }
                }
            }; 
            result = CommandResults.Successfully;
            result.ValueId = await cmd.ExecuteNonQueryAsync();
            return result;
        }
        catch(Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
            return result;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update ingredient set measure_id = $2, name = $3, image_path = $4 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = ingredient.Id },
                    new() { Value = ingredient.MeasureId },
                    new() { Value = ingredient.Name },
                    new() { Value = ingredient.ImagePath }
                }
            };
            result = await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.BadRequest; 
            return result;
        }
        catch(Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
            return result;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteIngredientAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from ingredient where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = id },
                }
            };
            result = await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.BadRequest; 
            return result;
        }
        catch(Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
            return result;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}