using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeRepository
{
    public Task<Recipe> GetRecipeAsync(int id);
    public Task<List<Recipe>> GetRecipesAsync();
    public Task<List<Recipe>> GetClientRecipesAsync(int clientId);
    public Task<CommandResult> AddRecipeAsync(Recipe recipe);
    public Task<CommandResult> UpdateRecipeAsync(Recipe recipe);
    public Task<CommandResult> DeleteRecipeAsync(Recipe recipe);
}