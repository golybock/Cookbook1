using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;

public interface IIngredientRepository
{
    public Task<Ingredient> GetIngredientAsync(int id);
    public Task<List<Ingredient>> GetIngredientsAsync();
    public Task<CommandResult> AddIngredientAsync(Ingredient ingredient);
    public Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient);
    public Task<CommandResult> DeleteIngredientAsync(int id);
}