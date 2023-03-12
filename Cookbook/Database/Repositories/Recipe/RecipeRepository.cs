using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Npgsql;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeRepository : RepositoryBase, IRecipeRepository
{
    public async Task<RecipeModel> GetRecipeAsync(int id)
    {
        var recipe = new RecipeModel();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeType.Id = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));

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
            return new RecipeModel();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeModel>> GetRecipesAsync()
    {
        var recipes = new List<RecipeModel>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipe = new RecipeModel();

                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeType.Id = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));

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
            return new List<RecipeModel>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeModel>> GetClientRecipesAsync(int clientId)
    {
        var recipes = new List<RecipeModel>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe where client_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = clientId}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipe = new RecipeModel();

                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                recipe.RecipeType.Id = reader.GetInt32(reader.GetOrdinal("recipe_type_id"));
                recipe.Name = reader.GetString(reader.GetOrdinal("name"));

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
            return new List<RecipeModel>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeAsync(RecipeModel recipe)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into recipe(client_id, recipe_type_id, name," +
                        " description, path_to_text_file, portion_count, cooking_time)" +
                        " VALUES ($1, $2, $3, $4, $5, $6, $7)" +
                        " returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipe.ClientId},
                    new NpgsqlParameter {Value = recipe.RecipeTypeId == -1 ? DBNull.Value : recipe.RecipeTypeId},
                    new NpgsqlParameter {Value = recipe.Name},
                    new NpgsqlParameter {Value = recipe.Description == null ? DBNull.Value : recipe.Description},
                    new NpgsqlParameter {Value = recipe.PathToTextFile == null ? DBNull.Value : recipe.PathToTextFile},
                    new NpgsqlParameter {Value = recipe.CookingTime},
                    new NpgsqlParameter {Value = recipe.PortionCount}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                result.Value = recipe;
            }

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

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update recipe set recipe_type_id = $2, name = $3," +
                        " description = $4, path_to_text_file = $5," +
                        " portion_count = $6, cooking_time = $7" +
                        " where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipe.Id},
                    new NpgsqlParameter {Value = recipe.RecipeTypeId == 0 ? 1 : recipe.RecipeTypeId},
                    new NpgsqlParameter {Value = recipe.Name == string.Empty ? "" : recipe.Name},
                    new NpgsqlParameter {Value = recipe.Description == string.Empty ? "" : recipe.Description},
                    new NpgsqlParameter {Value = recipe.PathToTextFile == string.Empty ? "" : recipe.PathToTextFile},
                    new NpgsqlParameter {Value = recipe.PortionCount == 0 ? DBNull.Value : recipe.PortionCount},
                    new NpgsqlParameter {Value = recipe.CookingTime == 0 ? DBNull.Value : recipe.CookingTime}
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

    public async Task<CommandResult> DeleteRecipeAsync(int id)
    {
        return await DeleteAsync("recipe", id);
    }
}