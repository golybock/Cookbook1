using System.Windows.Controls;
using Cookbook.ViewModels.Auth;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.Auth;

public partial class LoginPage : Page
{
    public LoginPage(Frame frame)
    {
        InitializeComponent();
        DataContext = new LoginViewModel(frame);
    }
}