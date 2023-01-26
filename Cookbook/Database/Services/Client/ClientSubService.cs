using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.Client;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Login;

namespace Cookbook.Database.Services.Client;

public class ClientSubService : IClientSubService
{
    private readonly ClientSubRepository _clientSubRepository;

    public ClientSubService()
    {
        _clientSubRepository = new ClientSubRepository();
    }

    public async Task<ClientSub> GetClientSubAsync(int id)
    {
        return await _clientSubRepository.GetClientSubAsync(id);
    }

    public async Task<List<ClientSub>> GetClientSubsAsync(int clientId)
    {
        return await _clientSubRepository.GetClientSubsAsync(clientId);
    }

    public async Task<List<ClientSub>> GetSubsClientAsync(int clientId)
    {
        return await _clientSubRepository.GetSubsClientAsync(clientId);
    }

    public async Task<CommandResult> AddClientSubAsync(ClientSub clientSub)
    {
        return await _clientSubRepository.AddClientSubAsync(clientSub);
    }

    public async Task<CommandResult> UpdateClientSubAsync(ClientSub clientSub)
    {
        return await _clientSubRepository.UpdateClientSubAsync(clientSub);
    }

    public async Task<CommandResult> DeleteClientSubAsync(int id)
    {
        return await _clientSubRepository.DeleteClientSubAsync(id);
    }
}