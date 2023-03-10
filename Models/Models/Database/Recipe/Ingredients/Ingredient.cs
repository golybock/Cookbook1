namespace Models.Models.Database.Recipe.Ingredients;

public partial class Ingredient
{
    public int Id { get; set; }

    public int MeasureId { get; set; }

    public string Name { get; set; } = string.Empty;

    public Measure? Measure { get; set; } = new ();

    public override string ToString() =>
        $"{Name} \t {Measure?.Name}";
}