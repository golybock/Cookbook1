namespace Models.Models.Database;

public static class CommandResults
{
    public static CommandResult Successfully =>
        new CommandResult() { Code = 100, Description = "Успешно" };

    public static CommandResult ErrorConnection =>
        new CommandResult() { Code = 101, Description = "Ошибка подключения" };

    public static CommandResult BadRequest =>
        new CommandResult() { Code = 102, Description = "Ошибка выполнения запроса" };

    public static CommandResult NotFulfilled =>
        new CommandResult() { Code = 102, Description = "Не выполнено"} ;
}