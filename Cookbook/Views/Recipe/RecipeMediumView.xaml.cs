using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cookbook.AdaptiveWrap;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeMediumView : UserControl
{

    public RecipeMediumView()
    {
        InitializeComponent();
        var recipe = DataContext as RecipeModel;
    }

    public RecipeMediumView(RecipeModel recipe)
    {
        DataContext = recipe;
    }
    
    public delegate void LikeClickedEvent();
    public event LikeClickedEvent LikeClicked;
    

    // private void LikeButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    // {
    //     if (_recipe != null)
    //         if (_recipe.IsLiked == false)
    //         {
    //             _recipe.IsLiked = true;
    //             LikeButton.Background = Brushes.PaleVioletRed;
    //         }
    //         else
    //         {
    //             _recipe.IsLiked = false;
    //             LikeButton.Background = Brushes.White;
    //         }
    //             
    // }
}