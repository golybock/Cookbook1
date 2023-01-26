using Cookbook.Models.Login;

namespace Cookbook.Models;

public static class LoginResults
{
    // 100 - некоректные данные
    // 200 - null данные

    public static LoginResult InvalidLogin => 
        new LoginResult() { Code = 101, Result = false, Description = "Неверный логин" };

    public static LoginResult InvalidPassword =>
        new LoginResult() { Code = 102, Result = false, Description = "Неверный пароль" };

    public static LoginResult EmptyLogin =>
        new LoginResult() {Code = 201, Result = false, Description = "Введите логин" };

    public static LoginResult EmptyPassword => 
        new LoginResult() { Code = 202, Result = false, Description = "Введите пароль" };
    
    public static LoginResult EmptyData =>
        new LoginResult() { Code = 203, Result = false, Description = "Введите данные" };

    public static LoginResult Successfully =>
        new LoginResult() { Code = 100, Result = true, Description = "Успешно"};
}