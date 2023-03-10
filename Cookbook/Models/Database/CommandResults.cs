namespace Cookbook.Models.Database;

public static class CommandResults
{
    public static CommandResult Successfully => new() {Code = 100, Result = true, Description = "Успешно"};

    public static CommandResult ErrorConnection =>
        new() {Code = 101, Result = false, Description = "Ошибка подключения"};

    public static CommandResult BadRequest => new() {Code = 102, Result = false, Description = "Неверные данные"};

    public static CommandResult NotFulfilled => new() {Code = 102, Result = false, Description = "Не выполнено"};
}