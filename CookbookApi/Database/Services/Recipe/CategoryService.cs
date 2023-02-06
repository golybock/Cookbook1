﻿using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Recipe;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces;

namespace CookbookApi.Database.Services.Recipe;

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