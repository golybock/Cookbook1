using Cookbook.Models.Database;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace CookbookApi.Database.Services.Interfaces;

public interface IRecipeService
{
    public Task<RecipeModel> GetRecipeAsync(int id);
    public Task<List<RecipeModel>> GetRecipesAsync();
    public Task<List<RecipeModel>> GetClientRecipes(int clientId);
    public Task<List<RecipeModel>> GetClientFavRecipes(int clientId);
    public Task<CommandResult> AddRecipeAsync(RecipeModel recipe);
    public Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe);
    public Task<CommandResult> DeleteRecipeAsync(int id);
}