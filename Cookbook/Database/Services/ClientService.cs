using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe.Review;
using Cookbook.Models.Database.Client;
using Models.Models.Database;
using Models.Models.Login;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class ClientService : IClientService
{
    private readonly ClientModel _client;
    private readonly Client.ClientService _clientService;
    private readonly RecipeService _recipeService;
    private readonly ReviewService _reviewService;
    private readonly ClientImageService _clientImageService;
    private readonly ClientFavService _clientFavService;
    private readonly ClientSubService _clientSubService;

    public ClientService()
    {
        _client = new ClientModel();
        _recipeService = new RecipeService(_client);
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientSubService = new ClientSubService();
        _clientService = new Client.ClientService();
    }
    
    public ClientService(ClientModel client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientSubService = new ClientSubService();
        _clientService = new Client.ClientService();
    }
    
    public async Task<List<ClientModel>> GetClients()
    {
        List<ClientModel> clients = await _clientService.GetClientsAsync();

        foreach (var client in clients)
        {
            client.IsLiked =
                await _clientSubService.ClientIsLiked(_client.Id, client.Id);
        }
        
        return clients;
    }
    
    public async Task<List<ClientModel>> GetClientSubs(int clientId)
    {
        List<ClientModel> clients = new List<ClientModel>();

        var subs = await _clientSubService.GetClientSubsAsync(clientId);

        foreach (var sub in subs)
        {
            ClientModel? client = await _clientService.GetClientAsync(sub.ClientId);
            
            if (client != null)
                clients.Add(client);
        }

        return clients;
    }

    public Task<CommandResult> AddClientToSub(int clientId)
    {
        return
            _clientSubService
                .AddClientSubAsync(
                    new ClientSub 
                        { 
                            ClientId = _client.Id,
                            Sub = clientId
                        }
                    );
    }
    
    // unused
    // ReSharper disable once UnusedMember.Global
    public async Task<CommandResult> DeleteClientFromSub(int clientId)
    {
        var sub = await _clientSubService.GetClientSubAsync(_client.Id, clientId);

        if (sub != null)
            return await _clientSubService.DeleteClientSubAsync(sub.Id);
        
        return CommandResults.BadRequest;
    }
    

    
    public async Task<CommandResult> DeleteSub(int subId)
    {
        return await _clientSubService.DeleteClientSubAsync(_client.Id, subId);
    }

    private async Task GetClientInfo(ClientModel client)
    {
        if (client.Id != -1 && client.Id > 0)
        {
            var recipes = _recipeService.GetClientRecipes(client.Id);
            var image =  _clientImageService.GetClientImageByClientIdAsync(client.Id);
            var reviews = _reviewService.GetClientReviewAsync(client.Id);
            var clientImages = _clientImageService.GetClientImagesAsync(client.Id);
            var favRecipes = _clientFavService.GetFavoriteRecipesAsync(client.Id);
            var clientSubOn = _clientSubService.GetClientSubsAsync(client.Id);
            var clientSubs = _clientSubService.GetSubsClientAsync(client.Id);
            
            client.Recipes = await recipes;
            client.Reviews = await reviews;
            client.ClientImages = await clientImages;
            client.FavoriteRecipes = await favRecipes;
            client.ClientSubOnClients = await clientSubOn;
            client.ClientSubs = await clientSubs;
            client.ClientImage = (await image)!;
        }
    }

}