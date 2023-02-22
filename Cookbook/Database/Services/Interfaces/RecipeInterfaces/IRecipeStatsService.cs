using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeStatsService
{
    public Task<RecipeStats?> GetRecipeStatsAsync(int id);
    public Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> DeleteRecipeStatsAsync(int id);
}