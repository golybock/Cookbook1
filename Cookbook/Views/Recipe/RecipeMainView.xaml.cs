using System;
using System.Windows.Controls;

namespace Cookbook.Views.Recipe;

public partial class RecipeMainView : UserControl
{
    public RecipeMainView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }
    
    public delegate void LikeClickedEvent(int id);

    public event LikeClickedEvent? LikeClicked;

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke(Int32.Parse(Id.Text));
    }
}