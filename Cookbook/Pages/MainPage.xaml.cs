using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        _recipeService = new RecipeService();

        _client = client;
        
        GetRecipes();
        
        InitializeComponent();
    }

    private async void GetRecipes()
    {
        Recipes = await _recipeService.GetRecipesAsync();
        
        DataContext = this;
    }

    private void RecipesListView_OnDeleteClicked()
    {
        var selectedItem = GetSelectedObject();
        
        if (selectedItem != null)
        {
            ShowAcceptDialog();
        }
    }

    private void RecipesListView_OnEditClicked()
    {
        var selectedItem = GetSelectedObject() as RecipeModel;

        if (NavigationService != null) 
            NavigationService.Navigate(
                new AddEditRecipePage(selectedItem)
            );
    }

    private void RecipesListView_OnOpenClicked()
    {
        var selectedItem = GetSelectedObject() as RecipeModel;

        if (NavigationService != null) 
            NavigationService.Navigate(
                new RecipePage(selectedItem)
                );
    }

    private void RecipesListView_OnLikeClicked()
    {
        var selectedItem = GetSelectedObject() as RecipeModel;

        if (selectedItem != null)
            _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = selectedItem.Id});
        
        MessageBox.Show("Лайк");
    }
    
    private async void ShowAcceptDialog()
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
            DeleteRecipe();
            #pragma warning restore CS4014
        }
    }
    
    private async Task DeleteRecipe()
    {
        var selectedItem = GetSelectedObject();

        if (selectedItem is RecipeModel recipe)
        {
            await _recipeService.DeleteRecipeAsync(recipe.Id);
            Recipes.Remove(recipe);
            DataContext = this;
        }
        
    }

    private object? GetSelectedObject()
    {
        return RecipesListView
            .RecipesListBox
            .SelectedItem;
    }
}