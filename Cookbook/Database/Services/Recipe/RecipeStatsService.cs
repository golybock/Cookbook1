using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeStatsService : IRecipeStatsService
{
    private readonly RecipeStatsRepository _recipeStatsRepository;

    public RecipeStatsService()
    {
        _recipeStatsRepository = new RecipeStatsRepository();
    }

    public async Task<RecipeStats> GetRecipeStatsAsync(int id)
    {
        if (id < 0)
            return new RecipeStats();

        return await _recipeStatsRepository.GetRecipeStatsAsync(id);
    }

    public async Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats)
    {
        if (recipeStats.RecipeId == 0)
            return CommandResults.BadRequest;

        return await _recipeStatsRepository.AddRecipeStatsAsync(recipeStats);
    }

    public async Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats? recipeStats)
    {
        if (recipeStats == null)
            return CommandResults.BadRequest;

        return await _recipeStatsRepository.UpdateRecipeStatsAsync(recipeStats);
    }

    public async Task<CommandResult> DeleteRecipeStatsAsync(int id)
    {
        if (id < 0)
            return CommandResults.BadRequest;

        return await _recipeStatsRepository.DeleteRecipeStatsAsync(id);
    }
}