using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Login;
using Models.Models.Database;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Database.Services.Interfaces;

public interface IRecipeService
{
    public Task<RecipeModel> GetRecipeAsync(int id);
    public Task<List<RecipeModel>> GetRecipesAsync();
    public Task<List<RecipeModel>> GetClientRecipes(int clientId);
    public Task<List<RecipeModel>> GetClientFavRecipes(int clientId);
    public Task<List<RecipeModel>> FindRecipesAsync(string searchString);
    public Task<CommandResult> AddRecipeAsync(RecipeModel? recipe);
    public Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe);
    public Task<CommandResult> DeleteRecipeAsync(int id);
}