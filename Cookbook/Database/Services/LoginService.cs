using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe.Review;
using Models.Models.Login;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class LoginService : ILoginService
{
    private readonly Client.ClientService _clientService;
    private readonly Recipe.RecipeService _recipeService;
    private readonly ReviewService _reviewService;
    private readonly ClientImageService _clientImageService;
    private readonly ClientFavService _clientFavService;

    public LoginService()
    {
        _recipeService = new Recipe.RecipeService();
        _clientImageService = new ClientImageService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _clientService = new Client.ClientService();
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
        
        if (currentClient?.Password != App.Hash(password))
            return LoginResults.InvalidPassword;
        
        LoginResult result = LoginResults.Successfully;
        
        await GetClientInfo(currentClient);
        
        result.Client = currentClient;
        
        return result;
    }
    
    private async Task GetClientInfo(ClientModel client)
    {
        if (client.Id != -1 && client.Id > 0)
        {
            var recipes = _recipeService.GetClientRecipesAsync(client.Id);
            var image =  _clientImageService.GetClientImageByClientIdAsync(client.Id);
            var reviews = _reviewService.GetClientReviewAsync(client.Id);
            var clientImages = _clientImageService.GetClientImagesAsync(client.Id);
            var favRecipes = _clientFavService.GetFavoriteRecipesAsync(client.Id);

            client.Recipes = await recipes;
            client.FavoriteRecipes = await favRecipes;
            client.ClientImage = (await image)!;
        }
    }
}