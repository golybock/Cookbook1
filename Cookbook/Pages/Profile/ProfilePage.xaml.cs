using Cookbook.ViewModels.Client;
using ModernWpf.Controls;
using Client = Models.Models.Database.Client.Client;
using Page = System.Windows.Controls.Page;

namespace Cookbook.Pages.Profile;

public partial class ProfilePage : Page
{
    public ProfilePage(Client client, Frame frame)
    {
        InitializeComponent();

        ClientMainView.DataContext = new ClientMainViewModel(client, frame);
    }

}