using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Client;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.Profile;

public partial class FavoriteRecipePage : Page
{
    public FavoriteRecipePage(Client client, Frame frame)
    {
        InitializeComponent();
        DataContext = new FavoriteRecipesViewModel(client, frame);
    }
}