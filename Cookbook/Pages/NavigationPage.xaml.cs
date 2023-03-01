using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Find;
using Cookbook.Pages.LoginRegister;
using Cookbook.Pages.Profile;
using Cookbook.Pages.Recipe;
using Cookbook.Pages.Settings;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using Client = Models.Models.Database.Client.Client;
using Page = System.Windows.Controls.Page;


namespace Cookbook.Pages;

public partial class NavigationPage : Page
{
    private readonly Client _client;
    private NavigationViewItem _lastItem;
    public Frame FirstFrame { get; set; }

    public NavigationPage(Client client, Frame frame)
    {
        FirstFrame = frame;
        _client = client; 
        InitializeComponent();
    }

    private void NavigationPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        MainFrame.NavigationService.Navigate(new MainPage(_client));
    }

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
        
        MainFrame.Navigate(GetPage(view));
        
        return true;
    }

    private Page? GetPage(string pageName)
    {
        if (pageName == "MainPage")
            return new MainPage(_client);
        if (pageName == "FindPage")
            return new FindPage(_client);
        if (pageName == "ProfilePage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new ProfilePage(_client);
        // if (pageName == "SubsPage")
        //     return new SubsPage(_client);
        if (pageName == "AddPostPage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new AddEditRecipePage(_client);
        if (pageName == "FavoritePostsPage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new FavoritePostsPage(_client);

        return new SettingsPage();
    }

    private void ExitButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (FirstFrame != null)
            FirstFrame.Navigate(new LoginPage(FirstFrame));
    }
}