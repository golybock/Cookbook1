using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeCategoryService : IRecipeCategoryService
{
    private readonly RecipeCategoryRepository _recipeCategoryRepository;

    public RecipeCategoryService()
    {
        _recipeCategoryRepository = new RecipeCategoryRepository();
    }
    
    public async Task<RecipeCategory?> GetRecipeCategoryAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _recipeCategoryRepository.GetRecipeCategoryAsync(id);
    }

    public async Task<RecipeCategory?> GetRecipeMainCategoryAsync(int recipeId)
    {
        if (recipeId <= 0)
            return null;
        
        return await _recipeCategoryRepository.GetRecipeMainCategoryAsync(recipeId);
    }

    public async Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId)
    {
        if (recipeId <= 0)
            return new List<RecipeCategory>();
        
        return await _recipeCategoryRepository.GetRecipeCategoriesAsync(recipeId);
    }

    public async Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        if (recipeCategory.CategoryId <= 0 ||
            recipeCategory.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeCategoryRepository.AddRecipeCategoryAsync(recipeCategory);
    }

    public async Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        if(recipeCategory.Id <= 0)
            return CommandResults.BadRequest;

        if (recipeCategory.CategoryId <= 0 ||
            recipeCategory.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeCategoryRepository.UpdateRecipeCategoryAsync(recipeCategory);
    }

    public async Task<CommandResult> DeleteRecipeCategoryAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeCategoryRepository.DeleteRecipeCategoryAsync(id);
    }
}