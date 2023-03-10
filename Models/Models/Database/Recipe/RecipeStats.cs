namespace Models.Models.Database.Recipe;

public partial class RecipeStats
{
    public int RecipeId { get; set; }

    public decimal Squirrels { get; set; }

    public decimal Fats { get; set; }

    public decimal Carbohydrates { get; set; }

    public decimal Kilocalories { get; set; }

    public override string ToString()
    {
        string kilocalories = $"Ккал: {Kilocalories}";
        string squirrels = $"Белки: {Squirrels}";

        string carbohydrates = $"Углеводы: {Carbohydrates}";
        string fats = $"Жиры: {Fats}";


        return $"{kilocalories}\t{squirrels}\n{carbohydrates}\t{fats}";
    }
}