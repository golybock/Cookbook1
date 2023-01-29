using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeCategoryService
{
    public Task<RecipeCategory> GetRecipeCategoryAsync(int id);
    public Task<List<RecipeCategory>> GetRecipeCategoriesAsync(int recipeId);
    public Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory);
    public Task<CommandResult> UpdateRecipeCategoryAsync(RecipeCategory recipeCategory);
    public Task<CommandResult> DeleteRecipeCategoryAsync(int id);
}