﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Database.Services.Recipe.Ingredients;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeIngredientService : IRecipeIngredientService
{
    private readonly RecipeIngredientRepository _recipeIngredientRepository;
    private readonly IngredientService _ingredientService;

    public RecipeIngredientService()
    {
        _ingredientService = new IngredientService();
        _recipeIngredientRepository = new RecipeIngredientRepository();
    }
    
    public async Task<RecipeIngredient?> GetRecipeIngredientAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _recipeIngredientRepository.GetRecipeIngredientAsync(id);
    }

    public async Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId)
    {
        if (recipeId <= 0)
            return new List<RecipeIngredient>();

        List<RecipeIngredient> recipeIngredients =
            await _recipeIngredientRepository.GetRecipeIngredientByRecipeAsync(recipeId);

        foreach (var recipeIngredient in recipeIngredients)
            recipeIngredient.Ingredient = await _ingredientService.GetIngredientAsync(recipeIngredient.IngredientId);

        return recipeIngredients;
    }

    public Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        return _recipeIngredientRepository.GetRecipeIngredientsAsync();
    }

    public async Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        if(recipeIngredient.IngredientId <=0 ||
           recipeIngredient.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        if(recipeIngredient.Count < 1)
            return CommandResults.BadRequest;
        
        return await _recipeIngredientRepository.AddRecipeIngredientAsync(recipeIngredient);
    }

    public async Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        if(recipeIngredient.Id <= 0)
            return CommandResults.BadRequest;
        
        if(recipeIngredient.IngredientId <=0 ||
           recipeIngredient.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        if(recipeIngredient.Count < 1)
            return CommandResults.BadRequest;
        
        return await _recipeIngredientRepository.UpdateRecipeIngredientAsync(recipeIngredient);
    }

    public async Task<CommandResult> DeleteRecipeIngredientAsync(int id)
    {
        if(id <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeIngredientRepository.DeleteRecipeIngredientAsync(id);
    }
}