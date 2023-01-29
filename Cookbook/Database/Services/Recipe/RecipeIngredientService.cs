using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Services.Recipe;

public class RecipeIngredientService : IRecipeIngredientService
{
    public Task<RecipeIngredient> GetRecipeIngredientAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<RecipeIngredient> GetRecipeIngredientByRecipeAsync(int recipeId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<RecipeIngredient>> GetRecipeIngredientAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> DeleteRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        throw new System.NotImplementedException();
    }
}