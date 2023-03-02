using System.Windows;
using Cookbook.Pages.Auth;

namespace Cookbook.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.NavigationService.Navigate(new LoginPage(ContentFrame));
        }
    }
}