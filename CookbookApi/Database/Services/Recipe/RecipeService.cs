using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using CookbookApi.Database.Repositories.Recipe;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace CookbookApi.Database.Services.Recipe;

public class RecipeService : IRecipeService
{
    private readonly RecipeRepository _recipeRepository;

    public RecipeService()
    {
        _recipeRepository = new RecipeRepository();
    }
    
    public Task<RecipeModel> GetRecipeAsync(int id)
    {
        return _recipeRepository.GetRecipeAsync(id);
    }

    public Task<List<RecipeModel>> GetRecipesAsync()
    {
        return _recipeRepository.GetRecipesAsync();
    }

    public Task<List<RecipeModel>> GetClientRecipesAsync(int clientId)
    {
        return _recipeRepository.GetClientRecipesAsync(clientId);
    }

    public Task<CommandResult> AddRecipeAsync(RecipeModel recipe)
    {
        return _recipeRepository.AddRecipeAsync(recipe);
    }

    public Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        return _recipeRepository.UpdateRecipeAsync(recipe);
    }

    public Task<CommandResult> DeleteRecipeAsync(int id)
    {
        return _recipeRepository.DeleteRecipeAsync(id);
    }
}