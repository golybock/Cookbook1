namespace Cookbook.Models.Database.Recipe;

public class RecipeStats
{
    public int RecipeId { get; set; }

    public decimal Squirrels { get; set; }

    public decimal Fats { get; set; }

    public decimal Carbohydrates { get; set; }

    public decimal Kilocalories { get; set; }

    public override string ToString()
    {
        var kilocalories = $"Ккал: {Kilocalories}";
        var squirrels = $"Белки: {Squirrels}";

        var carbohydrates = $"Углеводы: {Carbohydrates}";
        var fats = $"Жиры: {Fats}";


        return $"{kilocalories}\t{squirrels}\n{carbohydrates}\t{fats}";
    }
}