using Models.Models.Register.Password;

namespace Models.Models.Register;

public class RegisterResult
{
    public int Code { get; set; }
    public bool Result { get; set; }
    public string? Description { get; set; }
    public PasswordResult? PasswordResult { get; set; }
}