using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeCategoryService : IRecipeCategoryService
{
    private readonly RecipeCategoryRepository _recipeCategoryRepository;

    public RecipeCategoryService()
    {
        _recipeCategoryRepository = new RecipeCategoryRepository();
    }
    
    public Task<RecipeCategory> GetRecipeCategoryAsync(int id)
    {
        return _recipeCategoryRepository.GetRecipeCategoryAsync(id);
    }

    public Task<RecipeCategory> GetRecipeMainCategoryAsync(int recipeId)
    {
        return _recipeCategoryRepository.GetRecipeMainCategoryAsync(recipeId);
    }

    public Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId)
    {
        return _recipeCategoryRepository.GetRecipeCategoriesAsync(recipeId);
    }

    public Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        return _recipeCategoryRepository.AddRecipeCategoryAsync(recipeCategory);
    }

    public Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        return _recipeCategoryRepository.UpdateRecipeCategoryAsync(recipeCategory);
    }

    public Task<CommandResult> DeleteRecipeCategoryAsync(int id)
    {
        return _recipeCategoryRepository.DeleteRecipeCategoryAsync(id);
    }
}