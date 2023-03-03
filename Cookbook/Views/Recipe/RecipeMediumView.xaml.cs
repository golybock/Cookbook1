﻿using System;
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

    public RecipeMediumView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        Like.Execute(Int32.Parse(Id.Text));
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Delete.Execute(Int32.Parse(Id.Text));
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Edit.Execute(Int32.Parse(Id.Text));
    }

    private void RecipeMediumView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Open.Execute(Int32.Parse(Id.Text));;
    }
}