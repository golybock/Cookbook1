using System.Windows.Controls;
using System.Windows.Navigation;
using Cookbook.ViewModels.Registration;

namespace Cookbook.Pages.Auth;

public partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel();
    }
    
    public RegistrationPage(string login)
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel(login, NavigationService.GetNavigationService(this));
    }
}