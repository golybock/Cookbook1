using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Models.Models.Database;
using Models.Models.Database.Client;

namespace Cookbook.Database.Services.Client;

public class ClientImageService : IClientImageService
{
    private readonly ClientImageRepository _clientImageRepository;

    public ClientImageService()
    {
        _clientImageRepository = new ClientImageRepository();
    }

    public async Task<ClientImage?> GetClientImageAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _clientImageRepository.GetClientImageAsync(id);
    }

    public async Task<ClientImage?> GetClientImageByClientIdAsync(int clientId)
    {
        if (clientId <= 0)
            return null;
        
        return await _clientImageRepository.GetClientImageByClientIdAsync(clientId);
    }

    public async Task<List<ClientImage>> GetClientImagesAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<ClientImage>();
        
        return await _clientImageRepository.GetClientImagesAsync(clientId);
    }

    public async Task<CommandResult> AddClientImageAsync(ClientImage clientImage)
    {
        if(clientImage.ClientId == 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrEmpty(clientImage.ImagePath))
            return CommandResults.BadRequest;

        return await _clientImageRepository.AddClientImageAsync(clientImage);
    }

    public async Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage)
    {
        if(clientImage.Id <= 0)
            return CommandResults.BadRequest;
        
        if(clientImage.ClientId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrEmpty(clientImage.ImagePath))
            return CommandResults.BadRequest;

        if (!File.Exists(clientImage.ImagePath))
            return CommandResults.BadRequest;

        
        return await _clientImageRepository.UpdateClientImageAsync(clientImage);
    }

    public async Task<CommandResult> DeleteClientImageAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;

        return await _clientImageRepository.DeleteClientImageAsync(id);
    }
}