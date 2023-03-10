namespace Cookbook.Models.Database.Recipe;

public class RecipeType
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Тип рецепта: {Name}";
    }
}