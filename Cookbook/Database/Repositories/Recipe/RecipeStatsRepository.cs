using System;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeStatsRepository : RepositoryBase, IRecipeStatsRepository
{
    public async Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        var recipeStats = new RecipeStats();

        var con = GetConnection();

        try
        {
            await con.OpenAsync();

            var query = "select * from recipe_stats where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipeStats.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeStats.Squirrels = reader.GetDecimal(reader.GetOrdinal("squirrels"));
                recipeStats.Fats = reader.GetDecimal(reader.GetOrdinal("fats"));
                recipeStats.Carbohydrates = reader.GetDecimal(reader.GetOrdinal("carbohydrates"));
                recipeStats.Kilocalories = reader.GetDecimal(reader.GetOrdinal("kilocalories"));
            }

            return recipeStats;
        }
        catch (Exception e)
        {
            return new RecipeStats();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into recipe_stats(recipe_id, squirrels, fats, carbohydrates, kilocalories)" +
                        " values ($1, $2, $3, $4, $5)";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeStats.RecipeId},
                    new NpgsqlParameter {Value = recipeStats.Squirrels},
                    new NpgsqlParameter {Value = recipeStats.Fats},
                    new NpgsqlParameter {Value = recipeStats.Carbohydrates},
                    new NpgsqlParameter {Value = recipeStats.Kilocalories}
                }
            };

            result = CommandResults.Successfully;

            await cmd.ExecuteNonQueryAsync();

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

    public async Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query =
                "update recipe_stats set squirrels = $2, fats = $3, carbohydrates = $4, kilocalories = $5 where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeStats.RecipeId},
                    new NpgsqlParameter {Value = recipeStats.Squirrels},
                    new NpgsqlParameter {Value = recipeStats.Fats},
                    new NpgsqlParameter {Value = recipeStats.Carbohydrates},
                    new NpgsqlParameter {Value = recipeStats.Kilocalories}
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

    public async Task<CommandResult> DeleteRecipeStatsAsync(int id)
    {
        CommandResult result;

        var connection = GetConnection();

        try
        {
            connection.Open();

            var query = "delete from recipe_stats where recipe_id = $1";
            await using var cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = id}
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