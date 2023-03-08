﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeImageRepository
{
    
    public Task<RecipeImage?> GetRecipeImageAsync(int id);
    public Task<RecipeImage?> GetRecipeImageByRecipeAsync(int recipeId);
    public Task<List<RecipeImage>> GetRecipeImagesAsync(int recipeId);
    public Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage);
    public Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage);
    public Task<CommandResult> DeleteRecipeImagesByRecipeAsync(int id);
    public Task<CommandResult> DeleteRecipeImageAsync(int id);
}