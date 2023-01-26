using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models;
using Cookbook.Models.Database;
using Cookbook.Models.Login;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces.Client;

public interface IClientService
{
    public Task<LoginResult> Login(ClientModel client);
    public Task<LoginResult> Login(string login, string password);
    public Task<ClientModel?> GetClientAsync(int id);
    public Task<ClientModel?> GetClientAsync(string login);
    public Task<List<ClientModel>> GetClientsAsync();
    public Task<CommandResult> AddClientAsync(ClientModel client);
    public Task<CommandResult> UpdateClientAsync(ClientModel client);
    public Task<CommandResult> DeleteClientAsync(int id);
}