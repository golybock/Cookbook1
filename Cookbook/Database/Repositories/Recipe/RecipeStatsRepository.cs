using System;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Models.Models.Database;
using Models.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeStatsRepository : MainDbClass, IRecipeStatsRepository
{
    public async Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        RecipeStats recipeStats = new RecipeStats();
        
        var con = GetConnection();
        
        try
        {
            await con.OpenAsync();
            
            string query = $"select * from recipe_stats where recipe_id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            
            string query = $"insert into recipe_stats(recipe_id, squirrels, fats, carbohydrates, kilocalories)" +
                           $" values ($1, $2, $3, $4, $5)";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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

            await cmd.ExecuteNonQueryAsync();
            
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

    public async Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        CommandResult result;
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"update recipe_stats set squirrels = $2, fats = $3, carbohydrates = $4, kilocalories = $5 where recipe_id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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

    public async Task<CommandResult> DeleteRecipeStatsAsync(int id) =>
        await DeleteAsync("recipe_stats", id);

}