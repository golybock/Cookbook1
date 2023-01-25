namespace Cookbook.Models.Register;

public static class RegisterResults
{
    public static RegisterResult InvalidLogin =>
        new RegisterResult() {Code = 101, Result = false, Description = "Неверный логин"};

    public static RegisterResult InvalidPassword =>
        new RegisterResult() {Code = 102, Result = false, Description = "Неверный пароль"};

    public static RegisterResult InvalidData =>
        new RegisterResult() {Code = 103, Result = false, Description = "Неверный логин и пароль"};

    public static RegisterResult Successfully =>
        new RegisterResult() {Code = 100, Result = true, Description = "Успешно"};
}