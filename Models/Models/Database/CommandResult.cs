namespace Models.Models.Database;

public class CommandResult
{
    public CommandResult(int code, string description, bool result, object? value = null, int valueId = 0)
    {
        Code = code;
        Description = description;
        Result = result;
        Value = value;
        ValueId = valueId;
    }

    public CommandResult()
    {
        Value = new object();
    }
    
    public CommandResult(string description)
    {
        Description = description;
        Value = new object();
    }

    public int Code { get; set; }
    public bool Result { get; set; }
    public string? Description { get; set; }
    public object? Value { get; set; }
    public int ValueId { get; set; }
}