using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Database.Services;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Recipe;
using Microsoft.Xaml.Behaviors.Core;
using Models.Models.Database.Client;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    private readonly RecipeService _recipeService;
    private readonly Client _client;
    // ReSharper disable once MemberCanBePrivate.Global
    public List<RecipeModel> Recipes { get; set; } = null!;

    public MainPage(Client client)
    {
        _client = client;
        
        _recipeService = new RecipeService(_client);

        GetRecipes();
        
        InitializeComponent();
    }

    private async void GetRecipes()
    {
        Recipes = await _recipeService.GetRecipesAsync();
        
        DataContext = this;
    }

    private void RecipesListView_OnOpenClicked(int id)
    {
        var recipe = Recipes.FirstOrDefault(c => c.Id == id);

        if (NavigationService != null)
            if (recipe != null)
                NavigationService.Navigate(
                    new RecipePage(_client, id)
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
                    new AddEditRecipePage(recipe)
                );
    }
}