using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Recipe;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces;

namespace CookbookApi.Database.Services.Recipe;

public class RecipeStatsService : IRecipeStatsService
{
    private readonly RecipeStatsRepository _recipeStatsRepository;

    public RecipeStatsService()
    {
        _recipeStatsRepository = new RecipeStatsRepository();
    }
    
    public Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        return _recipeStatsRepository.GetRecipeStatsAsync(id);
    }

    public Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        return _recipeStatsRepository.AddRecipeStatsAsync(recipeStats);
    }

    public Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats)
    {
        return _recipeStatsRepository.UpdateRecipeStatsAsync(recipeStats);
    }

    public Task<CommandResult> DeleteRecipeStatsAsync(int id)
    {
        return _recipeStatsRepository.DeleteRecipeStatsAsync(id);
    }
}