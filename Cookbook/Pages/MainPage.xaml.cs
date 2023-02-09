using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Database.Services;
using Microsoft.Xaml.Behaviors.Core;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    private RecipeService _recipeService;
    public List<RecipeModel> Recipes { get; set; }

    public MainPage()
    {
        _recipeService = new RecipeService();
        
        GetRecipes();
        
        InitializeComponent();
    }

    private async void GetRecipes()
    {
        Recipes = await _recipeService.GetRecipesAsync();

        foreach (var recipe in Recipes)
        {
            recipe.Like += Like_OnClick;
        }
        
        DataContext = this;
    }
    
    private void Like_OnClick(object s, EventArgs e)
    {
        MessageBox.Show("aboba");
    }
    
    private void EditMenuItem_OnClick()
    {
        MessageBox.Show("aboba");
    }
    
    private void DeleteMenuItem_OnClick()
    {
        MessageBox.Show("aboba");
    }
    
}