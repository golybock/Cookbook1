using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Models.Models.Database;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces.ClientInterfaces;

public interface IClientService
{
    public Task<ClientModel?> GetClientAsync(int id);
    public Task<ClientModel?> GetClientAsync(string login);
    public Task<List<ClientModel>> GetClientsAsync();
    public Task<CommandResult> AddClientAsync(ClientModel client);
    public Task<CommandResult> UpdateClientAsync(ClientModel client);
    public Task<CommandResult> DeleteClientAsync(int id);
}