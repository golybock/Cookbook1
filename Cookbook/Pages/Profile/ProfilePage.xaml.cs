using System.Threading.Tasks;
using System.Windows.Controls;
using Cookbook.Database.Services;
using Client = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Profile;

public partial class ProfilePage : Page
{
    private Client _client;
    private readonly RecipeService _recipeService;

    public ProfilePage()
    {
        _client = new Client();
        _recipeService = new RecipeService(_client);
        DataContext = _client;
        InitializeComponent();
    }

    public ProfilePage(Client client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        LoadClientRecipes();
        InitializeComponent();
    }

    private async Task LoadClientRecipes()
    {
        _client.Recipes = await _recipeService.GetClientRecipes(_client.Id);
        DataContext = _client;
    }
    
}