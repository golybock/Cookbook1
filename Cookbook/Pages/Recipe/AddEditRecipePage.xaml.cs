﻿using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    // private CookbookContext _context = new CookbookContext();
    public RecipeModel Recipe;
    
    public AddEditRecipePage()
    {
        InitializeComponent();
        Recipe = new RecipeModel();
        DataContext = Recipe;
        MediumPreview.DataContext = Recipe;
    }
    
    public AddEditRecipePage(RecipeModel recipe)
    {
        InitializeComponent();
        Recipe = recipe;
        DataContext = Recipe;
        MediumPreview.DataContext = Recipe;
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {

    }

    private void OutError(string error)
    {
        ErrorTextBlock.Text = error;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }

    private void ClearError()
    {
        ErrorTextBlock.Text = null;
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
    

    private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ClearError();
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShowAcceptDialog();
    }

    private void PreviewButton_OnClick(object sender, RoutedEventArgs e)
    {
        // ворк
    }

    private void ClearPage()
    {
        Recipe = new RecipeModel();
        DataContext = Recipe;
    }

    private async void ShowAcceptDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Очистка ввода",
            Content = "Вы уверены, что хотите очистить все введенные данные?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Очистить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            ClearPage();
        
    } 
}