using System;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Profile;
using Cookbook.Pages.Recipe;
using Cookbook.Pages.RecipesPage;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using Page = System.Windows.Controls.Page;


namespace Cookbook.Pages;

public partial class NavigationPage : Page
{
    private readonly Client _client;
    private NavigationViewItem _lastItem;

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

//     private void HomeButton_OnClick(object sender, RoutedEventArgs e)
//     {
//         ClearNavigationService();
//         BackButton.Visibility = Visibility.Collapsed;
//         MainFrame.NavigationService.Navigate(new Recipe.RecipesPage());
//     
//     }
//
//     private void FavoriteButton_OnClick(object sender, RoutedEventArgs e)
//     {
//
//         BackButton.Visibility = Visibility.Visible;
//         MainFrame.NavigationService.Navigate(new Recipe.RecipesPage());
// }
//
//     private void ProfilePage_OnClick(object sender, RoutedEventArgs e)
//     {
//         BackButton.Visibility = Visibility.Visible;
//         MainFrame.NavigationService.Navigate(new ProfilePage(_client));
//     }
//
//     private void BackButton_OnClick(object sender, RoutedEventArgs e)
//     {
//         NavigationService.GoBack();
//         if(!MainFrame.NavigationService.CanGoBack)
//             BackButton.Visibility = Visibility.Collapsed;
//
//     }

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

    // private void AddRecipeButton_OnClick(object sender, RoutedEventArgs e)
    // {
    //     BackButton.Visibility = Visibility.Visible;
    //     MainFrame.NavigationService.Navigate(new AddEditRecipePage());
    // }

    private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        NavigationViewItem? item = args.InvokedItemContainer as NavigationViewItem;

        if (item == null || item == _lastItem)
        {
            return;    
        }

        var clickedView = item.Tag?.ToString();
        
        if (!NavigateView(clickedView)) return;
        
        _lastItem = item;
    }

    private bool NavigateView(string? view)
    {
        if (string.IsNullOrWhiteSpace(view))
            return false;
        
        MainFrame.Navigate(new ProfilePage(_client));
        
        return true;
    }

    private Page? GetPage(string pageName)
    {
        if (pageName == "Home")
            return new Recipe.RecipesPage();
        if (pageName == "Profile")
            return new ProfilePage(_client);
        if (pageName == "Add")
            return new AddEditRecipePage();

        return null;
    }
    
    private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        throw new NotImplementedException();
    }
}