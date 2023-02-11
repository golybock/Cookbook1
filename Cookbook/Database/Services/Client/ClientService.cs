using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Models.Models.Database;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Client;

public class ClientService : IClientService
{
    private readonly ClientRepository _clientRepository;
    
    public ClientService()
    {
        _clientRepository = new ClientRepository();
    }
    
    public async Task<ClientModel?> GetClientAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _clientRepository.GetClientAsync(id);
    }

    public async Task<ClientModel?> GetClientAsync(string login)
    {
        if (string.IsNullOrEmpty(login))
            return null;

        return await _clientRepository.GetClientAsync(login);
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        return await _clientRepository.GetClientsAsync();
    }

    public async Task<CommandResult> AddClientAsync(ClientModel client)
    {
        if(string.IsNullOrWhiteSpace(client.Login))
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(client.Password))
            return CommandResults.BadRequest;
            
        if(string.IsNullOrWhiteSpace(client.Name))
            return CommandResults.BadRequest;

        return await _clientRepository.AddClientAsync(client);
    }

    public async Task<CommandResult> UpdateClientAsync(ClientModel client)
    {
        if(client.Id <= 0)
            return CommandResults.BadRequest;

        if(string.IsNullOrWhiteSpace(client.Login))
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(client.Password))
            return CommandResults.BadRequest;
            
        if(string.IsNullOrWhiteSpace(client.Name))
            return CommandResults.BadRequest;
        
        return await _clientRepository.UpdateClientAsync(client);
    }

    public async Task<CommandResult> DeleteClientAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;

        return await _clientRepository.DeleteClientAsync(id);
    }
}