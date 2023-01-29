using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeStatsRepository : MainDbClass, IRecipeStatsRepository
{
    public async Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        connection.Open();
        try
        {
            RecipeStats recipeStats = new RecipeStats();
            string query = $"select * from recipe_stats where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeStats.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeStats.Squirrels = reader.GetDecimal(reader.GetOrdinal("squirrls"));
                recipeStats.Fats = reader.GetDecimal(reader.GetOrdinal("fats"));
                recipeStats.Carbohydrates = reader.GetDecimal(reader.GetOrdinal("carbohydrates"));
                recipeStats.Kilocalories = reader.GetDecimal(reader.GetOrdinal("kilocalories"));
            }
            
            return recipeStats;
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

    public async Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into recipe_stats(recipe_id, squirrels, fats, carbohydrates, kilocalories) values ($1, $2, $3, $4, $5) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = recipeStats.RecipeId },
                    new() { Value = recipeStats.Squirrels },
                    new() { Value = recipeStats.Fats },
                    new() { Value = recipeStats.Carbohydrates },
                    new() { Value = recipeStats.Kilocalories }
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

    public async Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update recipe_stats set squirrels = $2, fats = $3, carbohydrates = $4, kilocalories = $5 where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = recipeStats.RecipeId },
                    new() { Value = recipeStats.Squirrels },
                    new() { Value = recipeStats.Fats },
                    new() { Value = recipeStats.Carbohydrates },
                    new() { Value = recipeStats.Kilocalories }
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

    public async Task<CommandResult> DeleteRecipeStatsAsync(int id)
    {
        return await DeleteAsync("recipe_stats", id);
    }
}