using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeIngredientService
{
    
    public Task<RecipeIngredient?> GetRecipeIngredientAsync(int id);
    public Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId);
    public Task<List<RecipeIngredient>> GetRecipeIngredientsAsync();
    public Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient);
    public Task<CommandResult> DeleteRecipeIngredientAsync(int id);
    public Task<CommandResult> DeleteRecipeIngredientByRecipeAsync(int recipeId);
}