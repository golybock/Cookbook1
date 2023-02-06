using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Recipe;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces;

namespace CookbookApi.Database.Services.Recipe;

public class RecipeImageService : IRecipeImageService
{
    private readonly RecipeImageRepository _recipeImageRepository;

    public RecipeImageService()
    {
        _recipeImageRepository = new RecipeImageRepository();
    }
    
    public Task<RecipeImage> GetRecipeImageAsync(int id)
    {
        return _recipeImageRepository.GetRecipeImageAsync(id);
    }

    public Task<RecipeImage> GetRecipeImageByRecipeAsync(int recipeId)
    {
        return _recipeImageRepository.GetRecipeImageByRecipeAsync(recipeId);
    }

    public Task<List<RecipeImage>> GetRecipeImagesAsync(int recipeId)
    {
        return _recipeImageRepository.GetRecipeImagesAsync(recipeId);
    }

    public Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage)
    {
        return _recipeImageRepository.AddRecipeImageAsync(recipeImage);
    }

    public Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage)
    {
        return _recipeImageRepository.UpdateRecipeImageAsync(recipeImage);
    }

    public Task<CommandResult> DeleteRecipeImageAsync(int id)
    {
        return _recipeImageRepository.DeleteRecipeImageAsync(id);
    }
}