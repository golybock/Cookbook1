﻿using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace CookbookApi.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeIngredientService
{
    
    public Task<RecipeIngredient> GetRecipeIngredientAsync(int id);
    public Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId);
    public Task<List<RecipeIngredient>> GetRecipeIngredientsAsync();
    public Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> DeleteRecipeIngredientAsync(int id);
}