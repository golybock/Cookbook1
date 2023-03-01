using System.Windows.Controls;
using Frame = ModernWpf.Controls.Frame;
using Cookbook.ViewModels.Registration;

namespace Cookbook.Pages.Auth;

public partial class RegistrationPage : Page
{
    public RegistrationPage(Frame frame)
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel(frame);
    }
    
    public RegistrationPage(string login, Frame frame)
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel(login, frame);
    }
}