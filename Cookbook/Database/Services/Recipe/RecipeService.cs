using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;

namespace Cookbook.Database.Services.Recipe;

public class RecipeService : IRecipeService
{
    public Task<Models.Database.Recipe.Recipe> GetRecipeAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Models.Database.Recipe.Recipe>> GetRecipesAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Models.Database.Recipe.Recipe>> GetClientRecipesAsync(int clientId)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> AddRecipeAsync(Models.Database.Recipe.Recipe recipe)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> UpdateRecipeAsync(Models.Database.Recipe.Recipe recipe)
    {
        throw new System.NotImplementedException();
    }

    public Task<CommandResult> DeleteRecipeAsync(Models.Database.Recipe.Recipe recipe)
    {
        throw new System.NotImplementedException();
    }
}