namespace Cookbook.Models.Database.Recipe;

public class RecipeCategory
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int CategoryId { get; set; }
}