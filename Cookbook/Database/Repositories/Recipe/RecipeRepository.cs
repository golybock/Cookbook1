using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeRepository : MainDbClass, IRecipeRepository
{
    public async Task<RecipeModel> GetRecipeAsync(int id)
    {
        var con = GetConnection();
        con.Open();
        try
        {
            RecipeModel recipe = new RecipeModel();
            string query = $"select * from recipe where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeTypeId = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));
                recipe.DateOfCreation = reader.GetDateTime(reader.GetOrdinal("date_of_creation"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                recipe.Description = description == DBNull.Value ? null : description.ToString();
                var pathToTextFile = reader.GetValue(reader.GetOrdinal("path_to_text_file"));
                recipe.PathToTextFile = pathToTextFile == DBNull.Value ? null : pathToTextFile.ToString();
                recipe.PortionCount = reader.GetInt32(reader.GetOrdinal("portion_count"));
                recipe.CookingTime = reader.GetInt32(reader.GetOrdinal("cooking_time"));
            }
            
            return recipe;
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

    public async Task<List<RecipeModel>> GetRecipesAsync()
    {
        var con = GetConnection();
        con.Open();
        List<RecipeModel> recipes = new List<RecipeModel>();
        try
        {
            string query = $"select * from recipe";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                RecipeModel recipe = new RecipeModel();
                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeTypeId = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));
                recipe.DateOfCreation = reader.GetDateTime(reader.GetOrdinal("date_of_creation"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                recipe.Description = description == DBNull.Value ? null : description.ToString();
                var pathToTextFile = reader.GetValue(reader.GetOrdinal("path_to_text_file"));
                recipe.PathToTextFile = pathToTextFile == DBNull.Value ? null : pathToTextFile.ToString();
                recipe.PortionCount = reader.GetInt32(reader.GetOrdinal("portion_count"));
                recipe.CookingTime = reader.GetInt32(reader.GetOrdinal("cooking_time"));
                recipes.Add(recipe);
            }
            
            return recipes;
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

    public async Task<List<RecipeModel>> GetClientRecipesAsync(int clientId)
    {
        var con = GetConnection();
        con.Open();
        List<RecipeModel> recipes = new List<RecipeModel>();
        try
        {
            string query = $"select * from recipe where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = clientId } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                RecipeModel recipe = new RecipeModel();
                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeTypeId = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));
                recipe.DateOfCreation = reader.GetDateTime(reader.GetOrdinal("date_of_creation"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                recipe.Description = description == DBNull.Value ? null : description.ToString();
                var pathToTextFile = reader.GetValue(reader.GetOrdinal("path_to_text_file"));
                recipe.PathToTextFile = pathToTextFile == DBNull.Value ? null : pathToTextFile.ToString();
                recipe.PortionCount = reader.GetInt32(reader.GetOrdinal("portion_count"));
                recipe.CookingTime = reader.GetInt32(reader.GetOrdinal("cooking_time"));
                recipes.Add(recipe);
            }
            
            return recipes;
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

    public async Task<CommandResult> AddRecipeAsync(RecipeModel recipe)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"insert into recipe(client_id, recipe_type_id, name, description, path_to_text_file, cooking_time) VALUES ($1, $2, $3, $4, $5, $6) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipe.Id },
                    new() { Value = recipe.RecipeTypeId },
                    new() { Value = recipe.Name },
                    new() { Value = recipe.Description },
                    new() { Value = recipe.PathToTextFile },
                    new() { Value = recipe.PortionCount },
                    new() { Value = recipe.CookingTime }
                }
            }; 
            result = CommandResults.Successfully;
            result.ValueId = await cmd.ExecuteNonQueryAsync();
            recipe.Id = result.ValueId;
            result.Value = recipe;
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

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"update recipe set recipe_type_id = $2, name = $3," +
                           $" description = $4, path_to_text_file = $5," +
                           $" portion_count = $6, cooking_time = $7" +
                           $" where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipe.Id },
                    new() { Value = recipe.RecipeTypeId },
                    new() { Value = recipe.Name },
                    new() { Value = recipe.Description },
                    new() { Value = recipe.PathToTextFile },
                    new() { Value = recipe.PortionCount },
                    new() { Value = recipe.CookingTime }
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
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteRecipeAsync(int id)
    {
        return await DeleteAsync("recipe", id);
    }
}