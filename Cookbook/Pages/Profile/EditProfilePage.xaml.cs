using System.Windows.Controls;
using Cookbook.ViewModels.Client;
using Models.Models.Database.Client;
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