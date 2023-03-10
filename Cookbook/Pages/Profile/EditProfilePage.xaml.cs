using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Client;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.Profile;

public partial class EditProfilePage : Page
{
    public EditProfilePage(Client client, Frame frame)
    {
        InitializeComponent();
        DataContext = new ClientEditViewModel(client, frame);
    }
}