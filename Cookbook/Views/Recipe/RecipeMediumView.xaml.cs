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
        LikeButton.MouseDown += OnLikeClicked;
    }

    public delegate void LikeClickedEvent();
    public event LikeClickedEvent LikeClicked;
    
    protected virtual void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke();
    }
}