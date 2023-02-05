using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using CookbookApi.Database.Repositories.Client;
using CookbookApi.Database.Services.Interfaces.ClientInterfaces;

namespace CookbookApi.Database.Services.Client;

public class ClientImageService : IClientImageService
{
    private readonly ClientImageRepository _clientImageRepository;

    public ClientImageService()
    {
        _clientImageRepository = new ClientImageRepository();
    }

    public async Task<ClientImage?> GetClientImageAsync(int id)
    {
        return await _clientImageRepository.GetClientImageAsync(id);
    }

    public async Task<ClientImage?> GetClientImageByClientIdAsync(int clientId)
    {
        return await _clientImageRepository.GetClientImageByClientIdAsync(clientId);
    }

    public async Task<List<ClientImage?>> GetClientImagesAsync(int clientId)
    {
        return await _clientImageRepository.GetClientImagesAsync(clientId);
    }

    public async Task<CommandResult> AddClientImageAsync(ClientImage clientImage)
    {
        return await _clientImageRepository.AddClientImageAsync(clientImage);
    }

    public async Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage)
    {
        return await _clientImageRepository.UpdateClientImageAsync(clientImage);
    }

    public async Task<CommandResult> DeleteClientImageAsync(int id)
    {
        return await _clientImageRepository.DeleteClientImageAsync(id);
    }
}