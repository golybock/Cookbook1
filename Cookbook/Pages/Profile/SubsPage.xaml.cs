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
    // ReSharper disable once MemberCanBePrivate.Global
    public List<Client> Clients { get; set; } = null!;
    private readonly ClientService _clientService;
    private readonly ClientViewService _clientViewService;

    public SubsPage(Client client)
    {
        _clientViewService = new ClientViewService(client);
        _clientService = new ClientService(client);

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
        _clientViewService.OpenClicked(id, Clients, NavigationService);
    }

    private void ClientListView_OnLikeClicked(int id)
    {
        _clientViewService.LikeClicked(id, Clients);
        DataContext = this;
    }

}