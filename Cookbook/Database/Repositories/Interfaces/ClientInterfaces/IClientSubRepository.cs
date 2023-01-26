using System.Collections.Generic;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientSubRepository
{
    public ClientSub GetClientSub(int id);
    public List<ClientImage> GetClientSubs(int clientId);
    public CommandResult AddClientSub(ClientSub clientSub);
    public CommandResult UpdateClientSub(ClientSub clientSub);
    public CommandResult DeleteClientSub(int id);
}