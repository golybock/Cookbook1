using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeImageService : IRecipeImageService
{
    private readonly RecipeImageRepository _recipeImageRepository;

    public RecipeImageService()
    {
        _recipeImageRepository = new RecipeImageRepository();
    }
    
    public async Task<RecipeImage?> GetRecipeImageAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _recipeImageRepository.GetRecipeImageAsync(id);
    }

    public async Task<RecipeImage?> GetRecipeImageByRecipeAsync(int recipeId)
    {
        if (recipeId <= 0)
            return null;
        
        return await _recipeImageRepository.GetRecipeImageByRecipeAsync(recipeId);
    }

    public async Task<List<RecipeImage>> GetRecipeImagesAsync(int recipeId)
    {
        if (recipeId <= 0)
            return new List<RecipeImage>();
        
        return await _recipeImageRepository.GetRecipeImagesAsync(recipeId);
    }

    public async Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage)
    {
        if(recipeImage.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(recipeImage.ImagePath))
            return CommandResults.BadRequest;

        return await _recipeImageRepository.AddRecipeImageAsync(recipeImage);
    }

    public async Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage)
    {
        if (recipeImage.Id <= 0)
            return CommandResults.BadRequest;
        
        if(recipeImage.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(recipeImage.ImagePath))
            return CommandResults.BadRequest;
        
        if(!File.Exists(recipeImage.ImagePath))
            return CommandResults.BadRequest;

        return await _recipeImageRepository.UpdateRecipeImageAsync(recipeImage);
    }

    public async Task<CommandResult> DeleteRecipeImageAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeImageRepository.DeleteRecipeImageAsync(id);
    }
}