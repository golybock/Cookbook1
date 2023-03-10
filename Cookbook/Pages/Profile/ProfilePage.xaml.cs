using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Client;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.Profile;

public partial class ProfilePage : Page
{
    public ProfilePage(Client client, Frame frame)
    {
        InitializeComponent();

        ClientMainView.DataContext =
            new ClientMainViewModel(client, frame);
    }
}