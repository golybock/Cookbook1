using System.Threading.Tasks;
using Cookbook.Models.Register;
using Models.Models.Register.Password;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces;

public interface IRegisterService
{
    public PasswordResult PasswordValidate(string pass);
    public Task<RegisterResult> Register(ClientModel client);
}