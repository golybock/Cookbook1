namespace Models.Models.Database;

public static class CommandResults
{
    public static CommandResult Successfully =>
        new CommandResult() { Code = 100, Result = true, Description = "Успешно" };

    public static CommandResult ErrorConnection =>
        new CommandResult() { Code = 101, Result = false, Description = "Ошибка подключения" };

    public static CommandResult BadRequest =>
        new CommandResult() { Code = 102, Result = false, Description = "Ошибка выполнения запроса" };

    public static CommandResult NotFulfilled =>
        new CommandResult() { Code = 102, Result = false, Description = "Не выполнено"} ;
}