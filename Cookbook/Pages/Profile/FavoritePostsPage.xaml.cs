using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Recipe;
using ModernWpf.Controls;
using Client = Models.Models.Database.Client.Client;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe; 

namespace Cookbook.Pages.Profile;

public partial class FavoritePostsPage : Page
{
    private readonly Client _client;
    private readonly RecipeService _recipeService;
    public List<RecipeModel> Recipes { get; set; }

    public FavoritePostsPage(Client client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        Recipes = new List<RecipeModel>();

        GetRecipes();

        InitializeComponent();

        if (client.Id == -1)
        {
            // показываем что таких постов нет
            NothingShowView.Visibility = Visibility.Visible;
            RecipesListView.Visibility = Visibility.Collapsed;
        }
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