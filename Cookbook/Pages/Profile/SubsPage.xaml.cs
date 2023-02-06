using System.Windows.Controls;
using Cookbook.Models.Database.Client;

namespace Cookbook.Pages.Profile;

public partial class SubsPage : Page
{
    private Client _client;
    
    public SubsPage()
    {
        _client = new Client();
        InitializeComponent();
    }

    public SubsPage(Client client)
    {
        _client = client;
        InitializeComponent();
    }
}