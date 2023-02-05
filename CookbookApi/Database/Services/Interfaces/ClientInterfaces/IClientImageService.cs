using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace CookbookApi.Database.Services.Interfaces.ClientInterfaces;

public interface IClientImageService
{
    public Task<ClientImage?> GetClientImageAsync(int id);
    public Task<ClientImage?> GetClientImageByClientIdAsync(int clientId);
    public Task<List<ClientImage?>> GetClientImagesAsync(int clientId);
    public Task<CommandResult> AddClientImageAsync(ClientImage clientImage);
    public Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage);
    public Task<CommandResult> DeleteClientImageAsync(int id);
}