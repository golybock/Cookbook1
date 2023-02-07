using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Cookbook.Models;
using Cookbook.Models.Database;
using Cookbook.Models.Login;
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