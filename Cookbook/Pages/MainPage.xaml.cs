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
        
        DataContext = this;
    }
    
    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
    
    public class DeleteOnClick : ICommand
    {
        public bool CanExecute(object? parameter)
        {
            MessageBox.Show("Aboba");
            return true;
        }

        public void Execute(object? parameter)
        {
            MessageBox.Show("Aboba");
            return;
        }

        public event EventHandler? CanExecuteChanged;
    }
    
}