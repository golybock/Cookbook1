using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeIngredientRepository
{
    
    public Task<RecipeIngredient> GetRecipeIngredientAsync(int id);
    public Task<RecipeIngredient> GetRecipeIngredientByRecipeAsync(int recipeId);
    public Task<List<RecipeIngredient>> GetRecipeIngredientAsync();
    public Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> DeleteRecipeIngredientAsync(RecipeIngredient recipeIngredient);
}