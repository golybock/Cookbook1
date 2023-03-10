using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Register;
using Models.Models.Register.Password;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class RegisterService : IRegisterService
{
    private readonly ClientImageService _clientImageService;
    private readonly ClientService _clientService;

    public RegisterService()
    {
        _clientService = new ClientService();
        _clientImageService = new ClientImageService();
    }

    public async Task<RegisterResult> Register(ClientModel client)
    {
        if (client.Login == string.Empty &&
            client.Password == string.Empty)
            return RegisterResults.InvalidData;

        var passwordResult = PasswordValidate(client.Password);

        if (!passwordResult.Result)
            return new RegisterResult
            {
                Code = 102, Result = false,
                PasswordResult = passwordResult,
                Description = "Неверный пароль"
            };

        if (!await LoginValid(client.Login))
            return RegisterResults.InvalidLogin;

        var notHashedPass = client.Password;

        // хешируем пароль
        client.Password = App.Hash(client.Password);

        var сommandResult = await _clientService.AddClientAsync(client);

        if (сommandResult.Result)
        {
            // save image to docs
            var newClientImage = new ClientImage {ClientId = client.Id, ImagePath = client.NewImagePath};

            client.ClientImage.ClientId = client.Id;
            client.ClientImage.ImagePath = CopyImageToDocuments(newClientImage);

            var cmdResult = await _clientImageService.AddClientImageAsync(client.ClientImage);

            if (cmdResult.Result)
                client.ClientImage = (cmdResult.Value as ClientImage)!;

            return RegisterResults.Successfully;
        }


        client.Password = notHashedPass;
        var res = RegisterResults.InvalidData;
        res.Description = сommandResult.Description;

        return res;
    }

    public PasswordResult PasswordValidate(string password)
    {
        if (!PasswordNotNull(password))
            return PasswordResults.EmptyPassword;

        if (!PasswordHasDigit(password))
            return PasswordResults.NeedDigit;

        if (!PasswordHasSymbol(password))
            return PasswordResults.NeedSymbol;

        if (!PasswordHasUpper(password))
            return PasswordResults.NeedUpper;

        if (!PasswordLengthValid(password))
            return PasswordResults.NeedLength;


        return PasswordResults.Successfully;
    }

    private string? CopyImageToDocuments(ClientImage clientImage)
    {
        var documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients\\";

        var filePath = $"client_{clientImage.ClientId}_{App.GetTimeStamp()}.png";

        var writePath = documentsPath + filePath;

        if (File.Exists(clientImage.GetImagePath()))
        {
            File.Copy(clientImage.GetImagePath(), writePath);
            return filePath;
        }

        return null;
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

    private async Task<bool> LoginValid(string login)
    {
        var client = await _clientService.GetClientAsync(login);

        return client?.Id == 0;
    }
}