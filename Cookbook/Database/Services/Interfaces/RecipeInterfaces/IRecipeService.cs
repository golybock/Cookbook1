using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeService
{
    public Task<Recipe> GetRecipeAsync(int id);
    public Task<List<Recipe>> GetRecipesAsync();
    public Task<List<Recipe>> GetClientRecipesAsync(int clientId);
    public Task<CommandResult> AddRecipeAsync(Recipe recipe);
    public Task<CommandResult> UpdateRecipeAsync(Recipe recipe);
    public Task<CommandResult> DeleteRecipeAsync(Recipe recipe);
}