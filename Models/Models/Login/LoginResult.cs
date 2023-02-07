using Cookbook.Models.Database.Client;

namespace Models.Models.Login;

public class LoginResult
{
    public int Code { get; set; }
    public bool Result { get; set; }
    public string? Description { get; set; }
    public Client? Client { get; set; }
}