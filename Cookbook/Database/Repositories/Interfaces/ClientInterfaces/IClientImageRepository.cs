using System.Collections.Generic;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientImageRepository
{
    public ClientImage GetClientImage(int id);
    public List<ClientImage> GetClientImages();
    public CommandResult AddClientImage(ClientImage clientImage);
    public CommandResult UpdateClientImage(ClientImage clientImage);
    public CommandResult DeleteClientImage(int id);
}