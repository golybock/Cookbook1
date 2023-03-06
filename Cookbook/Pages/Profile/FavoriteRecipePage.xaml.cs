using System.Windows.Controls;
using Cookbook.ViewModels.Client;
using Models.Models.Database.Client;
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