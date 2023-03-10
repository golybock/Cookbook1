namespace Cookbook.Models.Register;

public static class RegisterResults
{
    public static RegisterResult InvalidLogin => new() {Code = 101, Result = false, Description = "Неверный логин"};

    public static RegisterResult InvalidPassword => new() {Code = 102, Result = false, Description = "Неверный пароль"};

    public static RegisterResult InvalidData =>
        new() {Code = 103, Result = false, Description = "Неверный логин и пароль"};

    public static RegisterResult Successfully => new() {Code = 100, Result = true, Description = "Успешно"};
}