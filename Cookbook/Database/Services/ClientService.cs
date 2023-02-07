using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe.Review;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Login;
using Cookbook.Models.Register;
using Cookbook.Models.Register.Password;
using Models.Models.Login;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class ClientService : IClientService
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

    public async Task<RegisterResult> Register(ClientModel client)
    {
        if (client.Login == String.Empty &&
            client.Password == string.Empty)
            return RegisterResults.InvalidData;
        
        PasswordResult passwordResult = PasswordValidate(client.Password);
        
        if (!passwordResult.Result)
        {
            return new RegisterResult()
            {
                Code = 102, Result = false,
                PasswordResult = passwordResult,
                Description = "Неверный пароль"
            };
        }

        if (!LoginValid(client.Login))
            return RegisterResults.InvalidLogin;
        
        client.Password = Hash(client.Password);

        client.Id = _clientService.AddClientAsync(client).Id;

        ClientImage clientImage = new ClientImage();

        clientImage.ClientId = client.Id;

        await _clientImageService.AddClientImageAsync(clientImage);

        return RegisterResults.Successfully;
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
    
    public PasswordResult PasswordValidate(string password)
    {
        if (!PasswordNotNull(password))
            return PasswordResults.EmptyPassword;
        
        if(!PasswordHasDigit(password))
            return PasswordResults.NeedDigit;

        if (!PasswordHasSymbol(password))
            return PasswordResults.NeedSymbol;

        if (!PasswordHasUpper(password))
            return PasswordResults.NeedUpper;

        if (!PasswordLengthValid(password))
            return PasswordResults.NeedLength;
        
        
        return PasswordResults.Successfully;

    }
    
    public bool PasswordValid(string password)
    {
        return PasswordNotNull(password) &&
               PasswordHasDigit(password) &&
               PasswordHasSymbol(password) &&
               PasswordHasUpper(password) &&
               PasswordLengthValid(password);
    }

    private bool PasswordNotNull(string password)
    {
        return !string.IsNullOrEmpty(password);
    }

    private bool PasswordHasDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private bool PasswordHasUpper(string password)
    {
        return password.Any(char.IsUpper);
    }

    private bool PasswordLengthValid(string password)
    {
        return password.Length >= 8;
    }

    private bool PasswordHasSymbol(string password)
    {
        return password.Any(char.IsPunctuation);
    }

    private bool LoginValid(string login)
    {
        return _clientService.GetClientAsync(login).Id == 0;
    }

}