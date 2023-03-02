using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Command;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeMediumView : UserControl
{

    public static readonly DependencyProperty DeleteProperty =
        DependencyProperty.Register(
            "Delete",
            typeof(RelayCommand<Int32>),
            typeof(UserControl));
    
    public static readonly DependencyProperty EditProperty =
        DependencyProperty.Register(
            "Edit",
            typeof(RelayCommand<Int32>),
            typeof(UserControl));

    public static readonly DependencyProperty OpenProperty =
        DependencyProperty.Register(
            "Open",
            typeof(RelayCommand<Int32>),
            typeof(UserControl));
    
    public static readonly DependencyProperty LikeProperty =
        DependencyProperty.Register(
            "Like",
            typeof(RelayCommand<Int32>),
            typeof(UserControl));
    
    public RelayCommand<Int32> DeleteClicked
    {
        get => (RelayCommand<Int32>) GetValue(DeleteProperty);
        set => SetValue(DeleteProperty, value);
    }

    public RelayCommand<Int32> EditClicked
    {
        get => (RelayCommand<Int32>) GetValue(EditProperty);
        set => SetValue(EditProperty, value);
    }

    public RelayCommand<Int32> LikeClicked
    {
        get => (RelayCommand<Int32>) GetValue(LikeProperty);
        set => SetValue(LikeProperty, value);
    }

    public RelayCommand<Int32> OpenClicked
    {
        get => (RelayCommand<Int32>) GetValue(OpenProperty);
        set => SetValue(OpenProperty, value);
    }

    public RecipeMediumView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked.Execute(Int32.Parse(Id.Text));
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        DeleteClicked.Execute(Int32.Parse(Id.Text));
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        EditClicked.Execute(Int32.Parse(Id.Text));
    }

    private void RecipeMediumView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        OpenClicked.Execute(Int32.Parse(Id.Text));;
    }
}