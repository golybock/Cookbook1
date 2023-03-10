using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Models.Login;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class LoginService : ILoginService
{
    private readonly ClientFavService _clientFavService;
    private readonly ClientImageService _clientImageService;
    private readonly ClientService _clientService;
    private readonly Recipe.RecipeService _recipeService;

    public LoginService()
    {
        _recipeService = new Recipe.RecipeService();
        _clientImageService = new ClientImageService();
        _clientFavService = new ClientFavService();
        _clientService = new ClientService();
    }

    public async Task<LoginResult> Login(string login, string password)
    {
        var currentClient = await _clientService.GetClientAsync(login);

        if (string.IsNullOrEmpty(login))
            return LoginResults.EmptyLogin;

        if (string.IsNullOrEmpty(password))
            return LoginResults.EmptyPassword;

        if (currentClient?.Id == 0)
            return LoginResults.InvalidLogin;

        if (currentClient?.Password != App.Hash(password))
            return LoginResults.InvalidPassword;

        var result = LoginResults.Successfully;

        await GetClientInfo(currentClient);

        result.Client = currentClient;

        return result;
    }

    private async Task GetClientInfo(ClientModel client)
    {
        if (client.Id != -1 && client.Id > 0)
        {
            var recipes = _recipeService.GetClientRecipesAsync(client.Id);
            var image = _clientImageService.GetClientImageByClientIdAsync(client.Id);
            var favRecipes = _clientFavService.GetFavoriteRecipesAsync(client.Id);

            client.Recipes = await recipes;
            client.FavoriteRecipes = await favRecipes;
            client.ClientImage = (await image)!;
        }
    }
}