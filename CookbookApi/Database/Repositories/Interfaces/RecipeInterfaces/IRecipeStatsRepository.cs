using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeStatsRepository
{
    public Task<RecipeStats> GetRecipeStatsAsync(int id);
    public Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> DeleteRecipeStatsAsync(int id);
}