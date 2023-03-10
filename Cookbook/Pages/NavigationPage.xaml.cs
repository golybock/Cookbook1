using System.Windows;
using System.Windows.Input;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Auth;
using Cookbook.Pages.Profile;
using Cookbook.Pages.Recipe;
using Cookbook.Pages.Search;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;

namespace Cookbook.Pages;

public partial class NavigationPage : Page
{
    private readonly Client _client;
    private NavigationViewItem _lastItem;

    public NavigationPage(Client client, Frame frame)
    {
        FirstFrame = frame;
        _client = client;
        InitializeComponent();
    }

    public Frame FirstFrame { get; set; }

    private void NavigationPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        MainFrame.NavigationService.Navigate(new MainPage(_client, MainFrame));
    }

    private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var item = args.InvokedItemContainer as NavigationViewItem;

        if (item == null || item == _lastItem) return;

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
            return new MainPage(_client, MainFrame);

        if (pageName == "FindPage")
            return new SearchPage(_client, MainFrame);

        if (pageName == "ProfilePage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new ProfilePage(_client, MainFrame);

        if (pageName == "AddPostPage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new EditRecipePage(_client, MainFrame);

        if (pageName == "FavoritePostsPage")
            if (_client.Id == -1)
                return new UnavaliabalePage();
            else
                return new FavoriteRecipePage(_client, MainFrame);

        return null;
    }

    private void ExitButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        FirstFrame.Navigate(new LoginPage(FirstFrame));
    }
}