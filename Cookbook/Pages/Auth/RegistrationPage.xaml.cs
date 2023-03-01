using System.Windows.Controls;
using Cookbook.ViewModels.Auth;
using Frame = ModernWpf.Controls.Frame;

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