using System.Windows;
using Cookbook.Database.Repositories;
using Cookbook.Pages.Auth;

namespace Cookbook.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        RepositoryBase db = new();

        if (db.TrustConnection())
            ContentFrame.NavigationService.Navigate(new LoginPage(ContentFrame));
    }
}