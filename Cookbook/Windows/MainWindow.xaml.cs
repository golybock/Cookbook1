using System.Windows;
using Cookbook.Pages.LoginRegister;

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