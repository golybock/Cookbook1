using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Client = Models.Models.Database.Client.Client;
using RecipeModel = Models.Models.Database.Recipe.Recipe; 

namespace Cookbook.Pages.Profile;

public partial class FavoritePostsPage : Page
{
    private Client _client;
    private RecipeService _recipeService;
    public List<RecipeModel> Recipes { get; set; }

    public FavoritePostsPage()
    {
        _client = new Client();
        _recipeService = new RecipeService(_client);
        Recipes = new List<RecipeModel>();

        InitializeComponent();
        
        // показываем что таких постов нет
        NothingShowView.Visibility = Visibility.Visible;
        RecipesListView.Visibility = Visibility.Collapsed;
    }
    
    public FavoritePostsPage(Client client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        Recipes = new List<RecipeModel>();

        GetRecipes();

        InitializeComponent();
    }

    private async Task GetRecipes()
    {
        Recipes = await _recipeService.GetClientFavRecipes(_client.Id);

        if (Recipes.Count == 0)
        {
            // показываем что таких постов нет
            NothingShowView.Visibility = Visibility.Visible;
            RecipesListView.Visibility = Visibility.Collapsed;
        }
        
        DataContext = this;
    }

}