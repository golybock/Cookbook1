using System.Collections.Generic;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientSubRepository
{
    public ClientSub GetClientSubAsync(int id);
    public List<ClientImage> GetClientSubsAsync(int clientId);
    public CommandResult AddClientSubAsync(ClientSub clientSub);
    public CommandResult UpdateClientSubAsync(ClientSub clientSub);
    public CommandResult DeleteClientSubAsync(int id);
}