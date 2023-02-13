using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Database.Services;
using Cookbook.Pages.Recipe;
using ModernWpf.Controls;
using Client = Models.Models.Database.Client.Client;
using Page = System.Windows.Controls.Page;

namespace Cookbook.Pages.Profile;

public partial class SubsPage : Page
{
    private Client _client;
    // ReSharper disable once MemberCanBePrivate.Global
    public List<Client> Clients { get; set; } = null!;
    private readonly ClientService _clientService;

    public SubsPage(Client client)
    {
        _client = client;
        _clientService = new ClientService(_client);

        GetClients();
        
        InitializeComponent();
    }

    private async Task GetClients()
    {
        Clients = await _clientService.GetClients();
        
        DataContext = this;
    }

    private void ClientListView_OnOpenClicked(int id)
    {
        var client = Clients.FirstOrDefault(c => c.Id == id);

        if (NavigationService != null)
            if (client != null)
                NavigationService.Navigate(
                    new ProfilePage(client)
                );
    }

    private void ClientListView_OnLikeClicked(int id)
    {
        var client = Clients.FirstOrDefault(c => c.Id == id);

        if (client!.IsLiked == true)
        {
            client.IsLiked = false;
            _clientService.DeleteClientFromSub(id);
        }
        else
        {
            client.IsLiked = true;
            _clientService.AddClientToSub(id);
        }

        DataContext = this;
    }

}