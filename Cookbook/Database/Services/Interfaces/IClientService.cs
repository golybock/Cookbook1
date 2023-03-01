using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Login;
using Models.Models.Login;
using Models.Models.Register;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces;

public interface IClientService
{
    public Task<List<ClientModel>> GetClientSubs(int clientId);
    // public void DeleteClient(int id);
}