namespace Cookbook.Models.Database.Recipe;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Категория: {Name}";
    }
}