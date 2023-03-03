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

    public ClientService()
    {
        _client = new ClientModel();
        _recipeService = new RecipeService(_client);
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientService = new Client.ClientService();
    }
    
    public ClientService(ClientModel client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientService = new Client.ClientService();
    }
    
    public async Task<List<ClientModel>> GetClients()
    {
        List<ClientModel> clients = await _clientService.GetClientsAsync();

        return clients;
    }

    private async Task GetClientInfo(ClientModel client)
    {
        if (client.Id != -1 && client.Id > 0)
        {
            var recipes = _recipeService.GetClientRecipes(client.Id);
            var image =  _clientImageService.GetClientImageByClientIdAsync(client.Id);
            var favRecipes = _clientFavService.GetFavoriteRecipesAsync(client.Id); ;
            
            client.Recipes = await recipes;
            client.FavoriteRecipes = await favRecipes;
            client.ClientImage = (await image)!;
        }
    }

}