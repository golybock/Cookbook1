using Models.Models.Register.Password;

namespace Cookbook.Models.Register;

public class RegisterResult
{
    public int Code { get; set; }
    public bool Result { get; set; }
    public string? Description { get; set; }
    public PasswordResult? PasswordResult { get; set; }
}