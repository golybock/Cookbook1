using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeMediumView : UserControl
{
    public delegate void DeleteClick(int id);
    public delegate void EditClick(int id);
    public delegate void OpenClick(int id);
    public delegate void LikeCLick(int id);
    public event DeleteClick? DeleteClicked;
    public event EditClick? EditClicked;
    public event LikeCLick? LikeClicked;
    public event OpenClick? OpenClicked;
    
    public RecipeMediumView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke(Int32.Parse(Id.Text));
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        DeleteClicked?.Invoke(Int32.Parse(Id.Text));
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        EditClicked?.Invoke(Int32.Parse(Id.Text));
    }

    private void RecipeMediumView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        OpenClicked?.Invoke(Int32.Parse(Id.Text));
    }
}