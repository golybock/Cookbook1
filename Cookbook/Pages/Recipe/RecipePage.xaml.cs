using System.Threading.Tasks;
using System.Windows.Markup;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Models.Models.Database.Client;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    private readonly Client _client;
    private RecipeModel _recipe;
    private readonly RecipeService _recipeService;
    
    public RecipePage(RecipeModel recipe, Client client)
    {
        _recipe = recipe;
        _client = client;
        _recipeService = new RecipeService(_client);
        
        DataContext = _recipe;
        
        InitializeComponent();
    }
    
    public RecipePage(Client client, int recipeId)
    {
        _client = client;
        _recipeService = new RecipeService(_client);

        GetRecipe(recipeId);
        
        InitializeComponent();
    }

    private async Task GetRecipe(int recipeId)
    {
        _recipe = await _recipeService.GetRecipeAsync(recipeId);
        DataContext = _recipe;
    }
    
    
}