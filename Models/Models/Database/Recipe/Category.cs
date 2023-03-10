namespace Models.Models.Database.Recipe;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString() =>
        $"Категория: {Name}";
}