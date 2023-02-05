using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces;

public interface ICategoryRepository
{
    public Task<Category> GetCategoryAsync(int id);
    public Task<List<Category>> GetCategoriesAsync();
    public Task<CommandResult> AddCategoryAsync(Category category);
    public Task<CommandResult> UpdateCategoryAsync(Category category);
    public Task<CommandResult> DeleteCategoryAsync(int id);
}