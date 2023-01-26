using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.Client;
using Cookbook.Models;
using Cookbook.Models.Database;
using Cookbook.Models.Login;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Client;

public class ClientService : IClientService
{
    private readonly ClientRepository _clientRepository;
    // private readonly ClientImageRepository _clientImageRepository;

    public ClientService()
    {
        _clientRepository = new ClientRepository();
        // _clientImageRepository = new ClientImageRepository();
    }

    public async Task<LoginResult> Login(ClientModel client)
    {
        ClientModel? currentClient = await _clientRepository.GetClientAsync(client.Login);

        // currentClient.ClientImages = await _clientImageRepository.GetClientImagesAsync(currentClient.Id);

        if(string.IsNullOrEmpty(client.Login))
            return LoginResults.EmptyLogin;
        
        if(string.IsNullOrEmpty(client.Password))
            return LoginResults.EmptyPassword;
        
        if (currentClient.Id == 0)
            return LoginResults.InvalidLogin;
        
        if (currentClient.Password != client.Password)
            return LoginResults.InvalidPassword;
        
        LoginResult result = LoginResults.Successfully;
        result.Client = currentClient;
        
        return result;
    }

    public async Task<LoginResult> Login(string login, string password)
    {
        ClientModel currentClient = await _clientRepository.GetClientAsync(login);

        // currentClient.ClientImages = await _clientImageRepository.GetClientImagesAsync(currentClient.Id);

        if(string.IsNullOrEmpty(login))
            return LoginResults.EmptyLogin;
        
        if(string.IsNullOrEmpty(password))
            return LoginResults.EmptyPassword;
        
        if (currentClient.Id == 0)
            return LoginResults.InvalidLogin;
        
        if (currentClient.Password != password)
            return LoginResults.InvalidPassword;
        
        LoginResult result = LoginResults.Successfully;
        result.Client = currentClient;
        
        return result;
    }

    public async Task<ClientModel?> GetClientAsync(int id)
    {
        return await _clientRepository.GetClientAsync(id);
    }

    public async Task<ClientModel?> GetClientAsync(string login)
    {
        return await _clientRepository.GetClientAsync(login);
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        return await _clientRepository.GetClientsAsync();
    }

    public async Task<CommandResult> AddClientAsync(ClientModel client)
    {
        return await _clientRepository.AddClientAsync(client);
    }

    public async Task<CommandResult> UpdateClientAsync(ClientModel client)
    {
        return await _clientRepository.UpdateClientAsync(client);
    }

    public async Task<CommandResult> DeleteClientAsync(int id)
    {
        return await _clientRepository.DeleteClientAsync(id);
    }
}