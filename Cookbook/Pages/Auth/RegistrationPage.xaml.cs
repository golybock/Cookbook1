using System.Windows.Controls;
using Cookbook.ViewModels.Registration;

namespace Cookbook.Pages.Auth;

public partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        DataContext = new RegistrationViewModel();
        InitializeComponent();
    }
    
    public RegistrationPage(string login)
    {
        DataContext = new RegistrationViewModel(login);
        InitializeComponent();
    }
}