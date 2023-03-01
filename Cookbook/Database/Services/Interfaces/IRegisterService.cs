using System.Threading.Tasks;
using Models.Models.Register;
using Models.Models.Register.Password;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces;

public interface IRegisterService
{
    public PasswordResult PasswordValidate(string pass);
    public Task<RegisterResult> Register(ClientModel client);
}