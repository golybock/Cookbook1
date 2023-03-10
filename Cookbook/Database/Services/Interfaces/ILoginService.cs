using System.Threading.Tasks;
using Cookbook.Models.Login;

namespace Cookbook.Database.Services.Interfaces;

public interface ILoginService
{
    public Task<LoginResult> Login(string login, string password);
}