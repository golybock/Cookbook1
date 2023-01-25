namespace Cookbook.Models.Database.Recipe.Ingredients;

public partial class Ingredient
{
    public int Id { get; set; }

    public int MeasureId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImagePath { get; set; }

    public virtual Measure Measure { get; set; } = null!;
    
}