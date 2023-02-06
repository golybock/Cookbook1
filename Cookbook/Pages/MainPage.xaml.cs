using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cookbook.Database.Services;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    private RecipeService _recipeService;
    private List<RecipeModel> _recipes;
    private ListBox _recipesListBox;

    public MainPage()
    {
        _recipeService = new RecipeService();
        _recipes = new List<RecipeModel>();
        
        InitializeComponent();

        _recipesListBox = RecipesListBox;
        
        LoadRecipes();
    }

    private async Task LoadRecipes()
    {
        _recipes = await _recipeService.GetRecipesAsync();
        _recipesListBox.ItemsSource = _recipes;
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}