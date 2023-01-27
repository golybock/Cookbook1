using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface ICategoryService
{
    public Task<Category> GetCategoryAsync(int id);
    public Task<CommandResult> AddCategoryAsync(Category category);
    public Task<CommandResult> UpdateCategoryAsync(Category category);
    public Task<CommandResult> DeleteCategoryAsync(Category category);
}