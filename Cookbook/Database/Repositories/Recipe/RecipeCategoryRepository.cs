using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeCategoryRepository : RepositoryBase, IRecipeCategoryRepository
{
    public async Task<RecipeCategory> GetRecipeCategoryAsync(int id)
    {
        var recipeCategory = new RecipeCategory();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe_categories where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
            }

            return recipeCategory;
        }
        catch
        {
            return new RecipeCategory();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId)
    {
        var recipeCategories = new List<RecipeCategory>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe_categories where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = recipeId}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipeCategory = new RecipeCategory();

                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));

                recipeCategories.Add(recipeCategory);
            }

            return recipeCategories;
        }
        catch
        {
            return new List<RecipeCategory>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<RecipeCategory> GetRecipeMainCategoryAsync(int recipeId)
    {
        var recipeCategory = new RecipeCategory();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe_categories where recipe_id = $1 limit 1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = recipeId}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeCategory.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeCategory.CategoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
            }

            return recipeCategory;
        }
        catch
        {
            return new RecipeCategory();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into recipe_categories(recipe_id, category_id)" +
                        " values ($1, $2) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeCategory.RecipeId},
                    new NpgsqlParameter {Value = recipeCategory.CategoryId}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) recipeCategory.Id = reader.GetInt32(reader.GetOrdinal("id"));

            return result;
        }
        catch (Exception e)
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

    public async Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update recipe_categories set recipe_id = $2, category_id = $3 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeCategory.Id},
                    new NpgsqlParameter {Value = recipeCategory.RecipeId},
                    new NpgsqlParameter {Value = recipeCategory.CategoryId}
                }
            };

            result =
                await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.NotFulfilled;

            return result;
        }
        catch (Exception e)
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

    public async Task<CommandResult> DeleteRecipeCategoryAsync(int id)
    {
        return await DeleteAsync("recipe_categories", id);
    }

    public async Task<CommandResult> DeleteRecipeCategoriesAsync(int recipeId)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "delete from recipe_categories where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeId}
                }
            };

            result = await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.NotFulfilled;

            return result;
        }
        catch (Exception e)
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
}