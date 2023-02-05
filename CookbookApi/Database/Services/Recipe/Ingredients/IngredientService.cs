using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using CookbookApi.Database.Repositories.Recipe.Ingredients;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces.IngredientsInterfaces;

namespace CookbookApi.Database.Services.Recipe.Ingredients;

public class IngredientService : IIngredientService
{
    private readonly IngredientRepository _ingredientRepository;

    public IngredientService()
    {
        _ingredientRepository = new IngredientRepository();
    }
    
    public Task<Ingredient> GetIngredientAsync(int id)
    {
        return _ingredientRepository.GetIngredientAsync(id);
    }

    public Task<List<Ingredient>> GetIngredientsAsync()
    {
        return _ingredientRepository.GetIngredientsAsync();
    }

    public Task<CommandResult> AddIngredientAsync(Ingredient ingredient)
    {
        return _ingredientRepository.AddIngredientAsync(ingredient);
    }

    public Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient)
    {
        return _ingredientRepository.UpdateIngredientAsync(ingredient);
    }

    public Task<CommandResult> DeleteIngredientAsync(int id)
    {
        return _ingredientRepository.DeleteIngredientAsync(id);
    }
}