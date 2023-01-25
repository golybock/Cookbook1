namespace Cookbook.Models.Database.Recipe;

public partial class RecipeStats
{
    public int RecipeId { get; set; }

    public decimal? Squirrels { get; set; }

    public decimal? Fats { get; set; }

    public decimal? Carbohydrates { get; set; }

    public decimal? Kilocalories { get; set; }
}