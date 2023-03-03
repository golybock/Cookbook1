using System.Windows.Controls;
using Models.Models.Database.Client;

namespace Cookbook.Pages.Profile;

public partial class EditProfilePage : Page
{
    public EditProfilePage(Client client)
    {
        InitializeComponent();
        DataContext = client;
    }
}