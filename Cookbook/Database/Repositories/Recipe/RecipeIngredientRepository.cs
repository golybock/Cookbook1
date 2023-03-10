using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeIngredientRepository : RepositoryBase, IRecipeIngredientRepository
{
    public async Task<RecipeIngredient> GetRecipeIngredientAsync(int id)
    {
        var recipeIngredient = new RecipeIngredient();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe_ingredients where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));
            }

            return recipeIngredient;
        }
        catch
        {
            return new RecipeIngredient();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId)
    {
        var con = GetConnection();
        con.Open();
        var recipeIngredients = new List<RecipeIngredient>();
        try
        {
            var query = "select * from recipe_ingredients where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = recipeId}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipeIngredient = new RecipeIngredient();

                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));

                recipeIngredients.Add(recipeIngredient);
            }

            return recipeIngredients;
        }
        catch
        {
            return new List<RecipeIngredient>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        var recipeIngredients = new List<RecipeIngredient>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from recipe_ingredients";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipeIngredient = new RecipeIngredient();

                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));

                recipeIngredients.Add(recipeIngredient);
            }

            return recipeIngredients;
        }
        catch
        {
            return new List<RecipeIngredient>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into recipe_ingredients(recipe_id, ingredient_id, count)" +
                        " values ($1, $2, $3) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeIngredient.RecipeId},
                    new NpgsqlParameter {Value = recipeIngredient.IngredientId},
                    new NpgsqlParameter {Value = recipeIngredient.Count}
                }
            };
            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));

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

    public async Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update recipe_ingredients set count = $2 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeIngredient.Id},
                    new NpgsqlParameter {Value = recipeIngredient.Count}
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

    public async Task<CommandResult> DeleteRecipeIngredientAsync(int id)
    {
        return await DeleteAsync("recipe_ingredients", id);
    }

    public async Task<CommandResult> DeleteRecipeIngredientByRecipeAsync(int recipeId)
    {
        CommandResult result;

        var connection = GetConnection();

        try
        {
            connection.Open();

            var query = "delete from recipe_ingredients where recipe_id = $1";
            await using var cmd = new NpgsqlCommand(query, connection)
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
            await connection.CloseAsync();
        }
    }
}