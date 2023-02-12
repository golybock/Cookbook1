using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Models.Models.Database;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeService
{
    public Task<RecipeModel?> GetRecipeAsync(int id);
    public Task<List<RecipeModel>> GetRecipesAsync();
    public Task<List<RecipeModel>> GetClientRecipesAsync(int clientId);
    public Task<CommandResult> AddRecipeAsync(RecipeModel? recipe);
    public Task<CommandResult> UpdateRecipeAsync(RecipeModel? recipe);
    public Task<CommandResult> DeleteRecipeAsync(int id);
}