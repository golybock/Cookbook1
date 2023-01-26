using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientRepository
{
    public Task<ClientModel> GetClient(int id);
    public Task<ClientModel> GetClient(string login);
    public Task<List<ClientModel>> GetClients();
    public Task<CommandResult> AddClient(ClientModel client);
    public Task<CommandResult> UpdateClient(ClientModel client);
    public Task<CommandResult> DeleteClient(int id);
}