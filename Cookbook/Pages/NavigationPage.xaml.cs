using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Profile;
using Cookbook.Pages.Recipe;
using Cookbook.Pages.RecipesPage;


namespace Cookbook.Pages;

public partial class NavigationPage : Page
{
    private readonly Client _client;

    public NavigationPage()
    {
        _client = new Client();
        InitializeComponent();
    }
    
    public NavigationPage(Client client)
    {
        _client = client;
        InitializeComponent();
    }

    private void HomeButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClearNavigationService();
        BackButton.Visibility = Visibility.Collapsed;
        MainFrame.NavigationService.Navigate(new Recipe.RecipesPage());
    
    }

    private void FavoriteButton_OnClick(object sender, RoutedEventArgs e)
    {

        BackButton.Visibility = Visibility.Visible;
        MainFrame.NavigationService.Navigate(new Recipe.RecipesPage());
}

    private void ProfilePage_OnClick(object sender, RoutedEventArgs e)
    {
        BackButton.Visibility = Visibility.Visible;
        MainFrame.NavigationService.Navigate(new ProfilePage(_client));
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
        if(!MainFrame.NavigationService.CanGoBack)
            BackButton.Visibility = Visibility.Collapsed;

    }

    private void NavigationPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        NavigationService.RemoveBackEntry();
        MainFrame.NavigationService.Navigate(new Recipe.RecipesPage());
    }

    private void ClearNavigationService()
    {
        while (MainFrame.NavigationService.CanGoBack) {
            try {
                MainFrame.NavigationService.RemoveBackEntry();
            } catch (Exception ex) {
                break;
            }
        }
    }

    private void AddRecipeButton_OnClick(object sender, RoutedEventArgs e)
    {
        BackButton.Visibility = Visibility.Visible;
        MainFrame.NavigationService.Navigate(new AddEditRecipePage());
    }
    
}