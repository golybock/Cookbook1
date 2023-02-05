using Cookbook.Models.Database;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace CookbookApi.Database.Services.Interfaces.ClientInterfaces;

public interface IClientService
{
    public Task<ClientModel?> GetClientAsync(int id);
    public Task<ClientModel?> GetClientAsync(string login);
    public Task<List<ClientModel>> GetClientsAsync();
    public Task<CommandResult> AddClientAsync(ClientModel client);
    public Task<CommandResult> UpdateClientAsync(ClientModel client);
    public Task<CommandResult> DeleteClientAsync(int id);
}