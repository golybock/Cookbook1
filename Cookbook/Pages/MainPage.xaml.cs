using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Database.Services;
using Models.Models.Database.Client;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    private readonly RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    public List<RecipeModel> Recipes { get; set; }

    public MainPage(Client client)
    {
        _recipeService = 
            new RecipeService(client);
        _recipesViewService =
            new RecipesViewService(client);

        InitializeComponent();
    }

    private async Task GetRecipes()
    {
        Recipes = await _recipeService.GetRecipesAsync();
        DataContext = this;
    }

    private void RecipesListView_OnOpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, Recipes, NavigationService);
    }

    private void RecipesListView_OnLikeClicked(int id)
    {
        _recipesViewService.LikeClicked(id, Recipes);
        DataContext = this;
    }

    private void RecipesListView_OnDeleteClicked(int id)
    {
        _recipesViewService.DeleteClicked(id, Recipes);
        DataContext = this;
    }

    private void RecipesListView_OnEditClicked(int id)
    {
        _recipesViewService.EditClicked(id, Recipes, NavigationService);
    }

    private void MainPage_OnLoaded(object sender, RoutedEventArgs e) => GetRecipes();
}