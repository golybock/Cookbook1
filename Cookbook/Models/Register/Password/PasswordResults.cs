namespace Models.Models.Register.Password;

public static class PasswordResults
{
    public static PasswordResult EmptyPassword => new() {Code = 101, Result = false, Description = "Требуется пароль"};

    public static PasswordResult NeedDigit =>
        new() {Code = 102, Result = false, Description = "Пароль должен содержать число"};

    public static PasswordResult NeedUpper =>
        new() {Code = 103, Result = false, Description = "Пароль должен содержать заглавную букву"};

    public static PasswordResult NeedSymbol =>
        new() {Code = 104, Result = false, Description = "Пароль должен содержать специальный символ"};

    public static PasswordResult NeedLength =>
        new() {Code = 105, Result = false, Description = "Пароль должен содержать 8 символов"};

    public static PasswordResult Successfully => new() {Code = 100, Result = true, Description = "Успешно"};
}