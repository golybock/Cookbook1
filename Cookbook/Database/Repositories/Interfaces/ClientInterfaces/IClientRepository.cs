using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientRepository
{
    public Task<ClientModel> GetClientAsync(int id);
    public Task<ClientModel> GetClientAsync(string login);
    public Task<List<ClientModel>> GetClientsAsync();
    public Task<CommandResult> AddClientAsync(ClientModel client);
    public Task<CommandResult> UpdateClientAsync(ClientModel client);
    public Task<CommandResult> DeleteClientAsync(int id);
}