using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeStatsRepository : MainDbClass, IRecipeStatsRepository
{
    public async Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public async Task<RecipeStats> GetRecipeStatsByRecipeAsync(int recipeId)
    {
        throw new System.NotImplementedException();
    }

    public async Task<List<RecipeStats>> GetRecipeStatsAsync()
    {
        throw new System.NotImplementedException();
    }

    public async Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        throw new System.NotImplementedException();
    }

    public async Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        throw new System.NotImplementedException();
    }

    public async Task<CommandResult> DeleteRecipeStatsAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from recipe_stats where recipe_id = $1";
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