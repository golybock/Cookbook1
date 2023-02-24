using Cookbook.Models.Database.Recipe.Ingredients;

namespace Models.Models.Database.Recipe.Ingredients;

public partial class Ingredient
{
    public int Id { get; set; }

    public int MeasureId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImagePath { get; set; }

    public Measure? Measure { get; set; } = new Measure();
    
}