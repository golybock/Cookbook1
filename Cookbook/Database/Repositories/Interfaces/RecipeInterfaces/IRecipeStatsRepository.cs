using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeStatsRepository
{
    public Task<RecipeStats?> GetRecipeStatsAsync(int id);
    public Task<CommandResult> AddRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> UpdateRecipeStatsAsync(RecipeStats recipeStats);
    public Task<CommandResult> DeleteRecipeStatsAsync(int id);
}