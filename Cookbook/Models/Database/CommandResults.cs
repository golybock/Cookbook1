namespace Cookbook.Models.Database;

public static class CommandResults
{
    public static CommandResult Successfully =>
        new CommandResult() { Code = 100, Description = "Successfully" };

    public static CommandResult ErrorConnection =>
        new CommandResult() { Code = 101, Description = "ErrorConnection" };

    public static CommandResult BadRequest =>
        new CommandResult() { Code = 102, Description = "BadRequest" };
}