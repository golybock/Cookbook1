using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Profile;
using Cookbook.Pages.Recipe;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class ClientViewService
{
    private readonly ClientService _clientService;

    public ClientViewService(ClientModel client)
    {
        _clientService = new ClientService(client);
    }
    
    public void LikeClicked(int id, List<ClientModel> clients)
    {
        var client = clients.FirstOrDefault(c => c.Id == id);

        if (client!.IsLiked == true)
        {
            client.IsLiked = false;
            _clientService.DeleteSub(id);
        }
        else
        {
            client.IsLiked = true;
            _clientService.AddClientToSub(id);
        }
    }

    public void OpenClicked(int id, List<ClientModel> clients, NavigationService? navigationService)
    {
        var client = clients.FirstOrDefault(c => c.Id == id);

        if (navigationService != null)
            if (client != null)
                navigationService.Navigate(
                    new ProfilePage(client)
                );
    }
}