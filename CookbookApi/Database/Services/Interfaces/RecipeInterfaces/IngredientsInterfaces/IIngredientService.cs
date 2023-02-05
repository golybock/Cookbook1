﻿using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;

namespace CookbookApi.Database.Services.Interfaces.RecipeInterfaces.IngredientsInterfaces;

public interface IIngredientService
{
    public Task<Ingredient> GetIngredientAsync(int id);
    public Task<List<Ingredient>> GetIngredientsAsync();
    public Task<CommandResult> AddIngredientAsync(Ingredient ingredient);
    public Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient);
    public Task<CommandResult> DeleteIngredientAsync(int id);
}