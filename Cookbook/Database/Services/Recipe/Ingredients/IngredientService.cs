using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Ingredients;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database;

namespace Cookbook.Database.Services.Recipe.Ingredients;

public class IngredientService : IIngredientService
{
    private readonly IngredientRepository _ingredientRepository;

    public IngredientService()
    {
        _ingredientRepository = new IngredientRepository();
    }
    
    public async Task<Ingredient?> GetIngredientAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _ingredientRepository.GetIngredientAsync(id);
    }

    public Task<List<Ingredient>> GetIngredientsAsync()
    {
        return _ingredientRepository.GetIngredientsAsync();
    }

    public async Task<CommandResult> AddIngredientAsync(Ingredient ingredient)
    {
        if(ingredient.MeasureId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(ingredient.Name))
            return CommandResults.BadRequest;
        
        return await _ingredientRepository.AddIngredientAsync(ingredient);
    }

    public async Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient)
    {
        if(ingredient.Id <= 0)
            return CommandResults.BadRequest;
        
        if(ingredient.MeasureId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(ingredient.Name))
            return CommandResults.BadRequest;
        
        return await _ingredientRepository.UpdateIngredientAsync(ingredient);
    }

    public async Task<CommandResult> DeleteIngredientAsync(int id)
    {
        if(id <= 0)
            return CommandResults.BadRequest; 
        
        return await _ingredientRepository.DeleteIngredientAsync(id);
    }
}