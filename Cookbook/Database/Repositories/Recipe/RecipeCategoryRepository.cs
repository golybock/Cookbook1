using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeCategoryRepository : MainDbClass, IRecipeCategoryRepository
{
    public async Task<RecipeCategory> GetRecipeCategoryAsync(int id)
    {
        connection.Open();
        try
        {
            RecipeCategory recipeCategory = new RecipeCategory();
            string query = $"select * from recipe_categories where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));

            }
            
            return recipeCategory;
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

    public async Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId)
    {
        connection.Open();
        try
        {
            List<RecipeCategory> recipeCategories = new List<RecipeCategory>();
            string query = $"select * from recipe_categories where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = recipeId } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                RecipeCategory recipeCategory = new RecipeCategory();
                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
                recipeCategories.Add(recipeCategory);
            }
            
            return recipeCategories;
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

    public async Task<RecipeCategory?> GetRecipeMainCategoryAsync(int recipeId)
    {
        connection.Open();
        try
        {
            RecipeCategory recipeCategory = new RecipeCategory();
            string query = $"select * from recipe_categories where recipe_id = $1 limit 1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = recipeId } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
            }
            
            return recipeCategory;
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

    public async Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into recipe_categories(recipe_id, category_id) values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = recipeCategory.RecipeId },
                    new() { Value = recipeCategory.CategoryId }
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

    public async Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update recipe_categories set recipe_id = $2, category_id = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = recipeCategory.Id },
                    new() { Value = recipeCategory.RecipeId },
                    new() { Value = recipeCategory.CategoryId }
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

    public async Task<CommandResult> DeleteRecipeCategoryAsync(int id)
    {
        return await DeleteAsync("recipe_categories", id);
    }
}