using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeIngredientService : IRecipeIngredientService
{
    private readonly RecipeIngredientRepository _recipeIngredientRepository;

    public RecipeIngredientService()
    {
        _recipeIngredientRepository = new RecipeIngredientRepository();
    }
    
    public Task<RecipeIngredient> GetRecipeIngredientAsync(int id)
    {
        return _recipeIngredientRepository.GetRecipeIngredientAsync(id);
    }

    public Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId)
    {
        return _recipeIngredientRepository.GetRecipeIngredientByRecipeAsync(recipeId);
    }

    public Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        return _recipeIngredientRepository.GetRecipeIngredientsAsync();
    }

    public Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        return _recipeIngredientRepository.AddRecipeIngredientAsync(recipeIngredient);
    }

    public Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        return _recipeIngredientRepository.UpdateRecipeIngredientAsync(recipeIngredient);
    }

    public Task<CommandResult> DeleteRecipeIngredientAsync(int id)
    {
        return _recipeIngredientRepository.DeleteRecipeIngredientAsync(id);
    }
}