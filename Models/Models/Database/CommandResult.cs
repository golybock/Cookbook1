namespace Models.Models.Database;

public class CommandResult
{
    public CommandResult(int code, string description, bool result, object? value = null)
    {
        Code = code;
        Description = description;
        Result = result;
        Value = value;
    }

    public CommandResult()
    {
        Value = new object();
    }

    public int Code { get; set; }
    public bool Result { get; set; }
    public string? Description { get; set; }
    public object? Value { get; set; }
}