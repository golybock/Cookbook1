using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Models.Models.Database;
using Models.Models.Database.Recipe.Ingredients;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class IngredientRepository : MainDbClass, IIngredientRepository
{
    public async Task<Ingredient?> GetIngredientAsync(int id)
    {
        Ingredient ingredient = new Ingredient();
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
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
            }
            
            return ingredient;
        }
        catch
        {
            return new Ingredient();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        
        var con = GetConnection();
        
        try
        {
            con.Open();
           
            string query = $"select * from ingredient";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                Ingredient ingredient = new Ingredient();
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                ingredient.MeasureId = reader.GetInt32(reader.GetOrdinal("measure_id"));
                ingredient.Name = reader.GetString(reader.GetOrdinal("name"));

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
        CommandResult result;
        
        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"insert into ingredient(measure_id, name)" +
                           $" values ($1, $2) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = ingredient.Measure.Id },
                    new() { Value = ingredient.Name },
                }
            }; 
            
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
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
        CommandResult result;
        
        var con = GetConnection();

        try
        {
            await con.OpenAsync();
            
            string query = $"update ingredient set measure_id = $2, name = $3 where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = ingredient.Id },
                    new() { Value = ingredient.MeasureId },
                    new() { Value = ingredient.Name }
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

    public async Task<CommandResult> DeleteIngredientAsync(int id) =>
        await DeleteAsync("ingredient", id);
}