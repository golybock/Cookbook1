using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cookbook.Database.Services;
using Client = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Profile;

public partial class SubsPage : Page
{
    private Client _client;
    // ReSharper disable once MemberCanBePrivate.Global
    public List<Client> Clients { get; set; } = null!;
    private readonly ClientService _clientService;

    public SubsPage()
    {
        _clientService = new ClientService();
        _client = new Client();
        
        GetClients();
        
        InitializeComponent();
    }
    
    public SubsPage(Client client)
    {
        _clientService = new ClientService();
        _client = client;
        
        GetClients();
        
        InitializeComponent();
    }

    private async Task GetClients()
    {
        Clients = await _clientService.GetClients();

        
        DataContext = this;
    }
    
}