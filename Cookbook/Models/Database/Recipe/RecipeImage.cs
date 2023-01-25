namespace Cookbook.Models.Database.Recipe;

public class RecipeImage
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string ImagePath { get; set; } = null!;

    public int ImageNumber { get; set; }
}