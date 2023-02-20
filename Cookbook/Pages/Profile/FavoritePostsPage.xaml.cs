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

    private void RecipesListView_OnOpenClicked(int id)
    {
        var recipe = Recipes.FirstOrDefault(c => c.Id == id);

        if (NavigationService != null)
            if (recipe != null)
                NavigationService.Navigate(
                    new RecipePage(recipe, _client)
                );
    }

    private void RecipesListView_OnLikeClicked(int id)
    {
        var recipe = Recipes.FirstOrDefault(c => c.Id == id);

        if (recipe!.IsLiked == true)
        {
            recipe.IsLiked = false;
            _recipeService.DeleteFavRecipes(id, _client.Id);
        }
        else
        {
            recipe.IsLiked = true;
            _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = id});
        }

        DataContext = this;
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
        var recipe = Recipes.FirstOrDefault(c => c.Id == id);

        if (recipe != null)
        {
            await _recipeService.DeleteRecipeAsync(recipe.Id);
            Recipes.Remove(recipe);
        }

        DataContext = this;
    }

    private void RecipesListView_OnDeleteClicked(int id)
    {
        ShowAcceptDialog(id);
    }

    private void RecipesListView_OnEditClicked(int id)
    {
        var recipe = Recipes.FirstOrDefault(c => c.Id == id);

        if (NavigationService != null)
            if (recipe != null)
                NavigationService.Navigate(
                    new AddEditRecipePage(recipe, _client)
                );
    }
    
}