using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Models.Models.Database;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeService : IRecipeService
{
    private readonly RecipeRepository _recipeRepository;

    public RecipeService()
    {
        _recipeRepository = new RecipeRepository();
    }
    
    public async Task<RecipeModel?> GetRecipeAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _recipeRepository.GetRecipeAsync(id);
    }

    public Task<List<RecipeModel>> GetRecipesAsync()
    {
        return _recipeRepository.GetRecipesAsync();
    }

    public async Task<List<RecipeModel>> GetClientRecipesAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<RecipeModel>();
        
        return await _recipeRepository.GetClientRecipesAsync(clientId);
    }

    public async Task<CommandResult> AddRecipeAsync(RecipeModel? recipe)
    {
        if(recipe == null)
            return CommandResults.BadRequest; 
        
        if(string.IsNullOrWhiteSpace(recipe.Name))
            return CommandResults.BadRequest;
        
        if(recipe.ClientId <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeRepository.AddRecipeAsync(recipe);
    }

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel? recipe)
    {
        if(recipe == null)
            return CommandResults.BadRequest; 
        
        if(string.IsNullOrWhiteSpace(recipe.Name))
            return CommandResults.BadRequest;
        
        if(recipe.ClientId <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeRepository.UpdateRecipeAsync(recipe);
    }

    public async Task<CommandResult> DeleteRecipeAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;
        
        return await _recipeRepository.DeleteRecipeAsync(id);
    }
}