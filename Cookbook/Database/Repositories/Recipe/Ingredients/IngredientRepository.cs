using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class IngredientRepository : MainDbClass, IIngredientRepository
{
    public async Task<Ingredient?> GetIngredientAsync(int id)
    {
        var con = GetConnection();
        
        try
        {
            Ingredient ingredient = new Ingredient();
            string query = $"select * from ingredient where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            await con.CloseAsync();
        }
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        var con = GetConnection();
        List<Ingredient> ingredients = new List<Ingredient>();
        try
        {
           
            string query = $"select * from ingredient";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
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
            return new List<Ingredient>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddIngredientAsync(Ingredient ingredient)
    {
        var con = GetConnection();
        
        CommandResult result;
        
        try
        {
            string query = $"insert into ingredient(measure_id, name, image_path)" +
                           $" values ($1, $2, $3) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = ingredient.MeasureId },
                    new() { Value = ingredient.Name },
                    new() { Value = ingredient.ImagePath }
                }
            }; 
            
            result = CommandResults.Successfully;
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                result.ValueId = reader.GetInt32(reader.GetOrdinal("id"));
            }

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
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient)
    {
        var con = GetConnection();
        
        CommandResult result;
        
        try
        {
            string query = $"update ingredient set measure_id = $2, name = $3, image_path = $4 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = ingredient.Id },
                    new() { Value = ingredient.MeasureId },
                    new() { Value = ingredient.Name },
                    new() { Value = ingredient.ImagePath }
                }
            };
            
            result =
                await cmd.ExecuteNonQueryAsync() > 0 ?
                    CommandResults.Successfully :
                    CommandResults.NotFulfilled;
            
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
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteIngredientAsync(int id)
    {
        return await DeleteAsync("ingredient", id);
    }
}