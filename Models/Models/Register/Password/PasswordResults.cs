namespace Cookbook.Models.Register.Password;

public static class PasswordResults
{
    public static PasswordResult EmptyPassword =>
        new PasswordResult() {Code = 101, Result = false, Description = "Требуется пароль"};
    
    public static PasswordResult NeedDigit =>
        new PasswordResult() { Code = 102, Result = false, Description = "Пароль должен содержать число" };

    public static PasswordResult NeedUpper =>
        new PasswordResult() { Code = 103, Result = false, Description = "Пароль должен содержать заглавную букву" };

    public static PasswordResult NeedSymbol =>
        new PasswordResult() { Code = 104, Result = false, Description = "Пароль должен содержать специальный символ"};

    public static PasswordResult NeedLength =>
        new PasswordResult() {Code = 105, Result = false, Description = "Пароль должен содержать 8 символов"};

    public static PasswordResult Successfully =>
        new PasswordResult() {Code = 100, Result = true, Description = "Успешно"};
}