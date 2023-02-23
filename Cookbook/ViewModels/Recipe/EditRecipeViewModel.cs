using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;


namespace Cookbook.ViewModels.Recipe;

public class EditRecipeViewModel
{
    public RecipeIngredient RecipeIngredient { get; set; }
    public RecipeModel Recipe { get; set; }
    // public List<Ingredient> Ingredients => GetIngredients().Result;
    public List<Measure> Measures { get; set; }
    // public List<Category> Categories => GetCategories().Result;
    // public List<RecipeType> RecipeTypes => GetRecipeTypes().Result;

    private readonly RecipeService _recipeService;

    public EditRecipeViewModel(RecipeModel recipe, ClientModel client)
    {
        Measures = new List<Measure>();
        RecipeIngredient = new RecipeIngredient();

        Recipe = recipe;

        _recipeService = new RecipeService(client);
    }

    public EditRecipeViewModel(ClientModel client)
    {
        Measures = new List<Measure>();
        RecipeIngredient = new RecipeIngredient();
        Recipe = new RecipeModel();

        _recipeService = new RecipeService(client);
    }

    private async Task<List<Category>> GetCategories()
    {
        return await _recipeService.GetCategoriesAsync();
    } 
    
    private async Task<List<Ingredient>> GetIngredients()
    {
        return await _recipeService.GetIngredientsAsync();
    }

    private async Task<List<RecipeType>> GetRecipeTypes()
    {
        return await _recipeService.GetRecipeTypes();
    }
    
}


