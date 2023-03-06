using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class CategoryService : ICategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService()
    {
        _categoryRepository = new CategoryRepository();
    }
    
    public Task<Category> GetCategoryAsync(int id)
    {
        return _categoryRepository.GetCategoryAsync(id);
    }

    public Task<List<Category>> GetCategories()
    {
        return _categoryRepository.GetCategoriesAsync();
    }

    public Task<CommandResult> AddCategoryAsync(Category category)
    {
        return _categoryRepository.AddCategoryAsync(category);
    }

    public Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        return _categoryRepository.UpdateCategoryAsync(category);
    }

    public Task<CommandResult> DeleteCategoryAsync(int id)
    {
        return _categoryRepository.DeleteCategoryAsync(id);
    }
}