using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeCategoryRepository
{
    public Task<RecipeCategory> GetRecipeCategoryAsync(int id);
    public Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId);
    public Task<RecipeCategory> GetRecipeMainCategoryAsync(int recipeId);
    public Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory);
    public Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory);
    public Task<CommandResult> DeleteRecipeCategoryAsync(int id);
}