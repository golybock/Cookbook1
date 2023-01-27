﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeImageService
{
    
    public Task<RecipeImage> GetRecipeImageAsync(int id);
    public Task<RecipeImage> GetRecipeImageByRecipeAsync(int recipeId);
    public Task<List<RecipeImage>> GetRecipeImageAsync();
    public Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage);
    public Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage);
    public Task<CommandResult> DeleteRecipeImageAsync(RecipeImage recipeImage);
}