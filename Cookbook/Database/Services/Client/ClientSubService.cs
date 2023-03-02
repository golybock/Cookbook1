using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Models.Models.Database;

namespace Cookbook.Database.Services.Client;

public class ClientSubService : IClientSubService
{
    private readonly ClientSubRepository _clientSubRepository;

    public ClientSubService()
    {
        _clientSubRepository = new ClientSubRepository();
    }

    public async Task<ClientSub?> GetClientSubAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _clientSubRepository.GetClientSubAsync(id);
    }

    public async Task<ClientSub?> GetClientSubAsync(int clientId, int subId)
    {
        return await _clientSubRepository.GetClientSubAsync(clientId, subId);
    }

    public async Task<List<ClientSub>> GetClientSubsAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<ClientSub>();
        
        return await _clientSubRepository.GetClientSubsAsync(clientId);
    }

    public async Task<List<ClientSub>> GetSubsClientAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<ClientSub>();
        
        return await _clientSubRepository.GetSubsClientAsync(clientId);
    }

    public async Task<bool> ClientIsLiked(int clientId, int subId)
    {
        return await _clientSubRepository.ClientIsLiked(clientId, subId);
    }

    public async Task<CommandResult> AddClientSubAsync(ClientSub clientSub)
    {
        if(clientSub.Sub <= 0 ||
           clientSub.ClientId <= 0)
            return CommandResults.BadRequest;

        return await _clientSubRepository.AddClientSubAsync(clientSub);
    }

    public async Task<CommandResult> UpdateClientSubAsync(ClientSub clientSub)
    {
        if(clientSub.Id <= 0)
            return CommandResults.BadRequest;

        if(clientSub.Sub <= 0 ||
           clientSub.ClientId <= 0)
            return CommandResults.BadRequest;
        
        return await _clientSubRepository.UpdateClientSubAsync(clientSub);
    }

    public async Task<CommandResult> DeleteClientSubAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;

        return await _clientSubRepository.DeleteClientSubAsync(id);
    }

    public async Task<CommandResult> DeleteClientSubAsync(int clientId, int id)
    {
        if (id <= 0 || clientId <= 0)
            return CommandResults.BadRequest;

        return await _clientSubRepository.DeleteClientSubAsync(clientId, id);
    }
}