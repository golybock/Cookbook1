namespace Models.Models.Database.Recipe;

public partial class RecipeType
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString() =>
        $"Тип рецепта: {Name}";
    
}