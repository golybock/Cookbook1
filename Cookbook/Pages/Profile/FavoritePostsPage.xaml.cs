using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Client = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Profile;

public partial class FavoritePostsPage : Page
{
    private Client _client;
    
    public FavoritePostsPage()
    {
        _client = new Client();
        InitializeComponent();
    }
    
    public FavoritePostsPage(Client client)
    {
        _client = client;
        InitializeComponent();
    }
}