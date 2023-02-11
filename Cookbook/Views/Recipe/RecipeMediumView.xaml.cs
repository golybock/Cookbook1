using System;
using System.Windows.Controls;
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

    public event LikeClickedEvent? LikeClicked;

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke();
    }
    
}