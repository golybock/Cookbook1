using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database.Recipe;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;


namespace Cookbook.ViewModels.Recipe;

public class EditRecipeViewModel
{
    public Ingredient Ingredient { get; set; }
    public RecipeModel Recipe { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Category> Categories { get; set; }
    public List<RecipeType> RecipeTypes { get; set; }

    private readonly RecipeService _recipeService;

    public EditRecipeViewModel(RecipeModel recipe, ClientModel client)
    {
        Ingredient = new Ingredient();
        Ingredients = new List<Ingredient>();
        Categories = new List<Category>();
        RecipeTypes = new List<RecipeType>();

        Recipe = recipe;

        _recipeService = new RecipeService(client);
    }

    public EditRecipeViewModel(ClientModel client)
    {
        Ingredient = new Ingredient();
        Recipe = new RecipeModel();
        Ingredients = new List<Ingredient>();
        Categories = new List<Category>();
        RecipeTypes = new List<RecipeType>();

        _recipeService = new RecipeService(client);
    }

    private async Task GetCategories()
    {
        Categories = await _recipeService.GetCategoriesAsync();
    } 
    
    private async Task GetIngredients()
    {
        Ingredients = await _recipeService.GetIngredientsAsync();
    } 
    
    private async 
    
}


