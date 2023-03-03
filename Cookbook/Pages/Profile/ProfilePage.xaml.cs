using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Recipe;
using Cookbook.Views.Client;
using ModernWpf.Controls;
using Client = Models.Models.Database.Client.Client;
using Page = System.Windows.Controls.Page;

namespace Cookbook.Pages.Profile;

public partial class ProfilePage : Page
{
    private Client _client;
    private readonly RecipeService _recipeService;

    public ProfilePage(Client client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        LoadClientRecipes();
        InitializeComponent();

        ClientMainView.DataContext = client;
    }

    private async void ShowAcceptDialog(int id)
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Удаление элемента",
            Content = "Вы уверены, что хотите удалить этот рецепт?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Удалить",
            DefaultButton = ContentDialogButton.Primary
        };
    
        if (await acceptDialog.ShowAsync() == ContentDialogResult.Primary)
        {
#pragma warning disable CS4014
            DeleteRecipe(id);
#pragma warning restore CS4014
        }
    }
    private async Task DeleteRecipe(int id)
    {
        var recipe = _client.Recipes.FirstOrDefault(c => c.Id == id);

        if (recipe != null)
        {
            await _recipeService.DeleteRecipeAsync(recipe.Id);
            _client.Recipes.Remove(recipe);
        }

        DataContext = _client;
    }
    
    
    private async Task LoadClientRecipes()
    {
        _client.Recipes = await _recipeService.GetClientRecipes(_client.Id);
        if (_client.Recipes.Count <= 0)
        {
            RecipesListView.Visibility = Visibility.Collapsed;
            NothingShowView.Visibility = Visibility.Visible;
        }
        DataContext = _client;
    }
    
}