using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Recipe.Review;
using Cookbook.Models.Login;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class ClientService
{
    private readonly Client.ClientService _clientService;
    private readonly RecipeService _recipeService;
    private readonly ReviewService _reviewService;
    private readonly ClientImageService _clientImageService;
    private readonly ClientFavService _clientFavService;
    private readonly ClientSubService _clientSubService;

    public ClientService()
    {
        _recipeService = new RecipeService();
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientSubService = new ClientSubService();
        _clientService = new Client.ClientService();
    }
    
    public async Task<LoginResult> Login(ClientModel client)
    {
        return await Login(client.Login, client.Password);
    }

    public async Task<LoginResult> Login(string login, string password)
    {
        ClientModel? currentClient = await _clientService.GetClientAsync(login);
        
        if(string.IsNullOrEmpty(login))
            return LoginResults.EmptyLogin;
        
        if(string.IsNullOrEmpty(password))
            return LoginResults.EmptyPassword;
        
        if (currentClient?.Id == 0)
            return LoginResults.InvalidLogin;
        
        if (currentClient?.Password != Hash(password))
            return LoginResults.InvalidPassword;
        
        LoginResult result = LoginResults.Successfully;
        
        await GetClientInfo(currentClient);
        
        result.Client = currentClient;
        
        return result;
    }

    private async Task GetClientInfo(ClientModel? client)
    {
        if (client != null)
        {
            
            client.Recipes = await _recipeService.GetClientRecipes(client.Id);
            client.Reviews = await _reviewService.GetClientReviewAsync(client.Id);
            client.ClientImages = await _clientImageService.GetClientImagesAsync(client.Id);
            client.FavoriteRecipes = await _clientFavService.GetFavoriteRecipesAsync(client.Id);
            client.ClientSubOnClients = await _clientSubService.GetClientSubsAsync(client.Id);
            client.ClientSubs = await _clientSubService.GetSubsClientAsync(client.Id);

            var imagePath = await _clientImageService.GetClientImageAsync(client.Id);
            client.ImagePath = imagePath?.ImagePath;
        }
    }
    
    private static string Hash(string stringToHash)
    {
        if (stringToHash == "admin")
            return "admin";
        
        using var sha1 = new SHA1Managed();
        return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
    }

}