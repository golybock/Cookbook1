using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Services.Interfaces.Client;

public interface IClientImageService
{
    public Task<ClientImage?> GetClientImageAsync(int id);
    public Task<ClientImage?> GetClientImageByClientIdAsync(int clientId);
    public Task<List<ClientImage?>> GetClientImagesAsync(int clientId);
    public Task<CommandResult> AddClientImageAsync(ClientImage clientImage);
    public Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage);
    public Task<CommandResult> DeleteClientImageAsync(int id);
}