using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeMediumView : UserControl
{
    public static readonly DependencyProperty DeleteProperty =
        DependencyProperty.Register(
            "Delete",
            typeof(ICommand),
            typeof(UserControl));

    public static readonly DependencyProperty EditProperty =
        DependencyProperty.Register(
            "Edit",
            typeof(ICommand),
            typeof(UserControl));

    public static readonly DependencyProperty OpenProperty =
        DependencyProperty.Register(
            "Open",
            typeof(ICommand),
            typeof(UserControl));

    public static readonly DependencyProperty LikeProperty =
        DependencyProperty.Register(
            "Like",
            typeof(ICommand),
            typeof(UserControl));

    public static readonly DependencyProperty PrintProperty =
        DependencyProperty.Register(
            "Print",
            typeof(ICommand),
            typeof(UserControl));

    public RecipeMediumView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    public ICommand Delete
    {
        get => (ICommand) GetValue(DeleteProperty);
        set => SetValue(DeleteProperty, value);
    }

    public ICommand Edit
    {
        get => (ICommand) GetValue(EditProperty);
        set => SetValue(EditProperty, value);
    }

    public ICommand Like
    {
        get => (ICommand) GetValue(LikeProperty);
        set => SetValue(LikeProperty, value);
    }

    public ICommand Open
    {
        get => (ICommand) GetValue(OpenProperty);
        set => SetValue(OpenProperty, value);
    }

    public ICommand Print
    {
        get => (ICommand) GetValue(PrintProperty);
        set => SetValue(PrintProperty, value);
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        Like.Execute(int.Parse(Id.Text));
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Delete.Execute(int.Parse(Id.Text));
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Edit.Execute(int.Parse(Id.Text));
    }

    private void RecipeMediumView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Open.Execute(int.Parse(Id.Text));
    }

    private void PrintMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Print.Execute(int.Parse(Id.Text));
    }
}