using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeStatsService : IRecipeStatsService
{
    public Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<RecipeStats> GetRecipeStatsByRecipeAsync(int recipeId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<RecipeStats>> GetRecipeStatsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> DeleteRecipeStatsAsync(RecipeStats recipeStats)
    {
        throw new System.NotImplementedException();
    }
}