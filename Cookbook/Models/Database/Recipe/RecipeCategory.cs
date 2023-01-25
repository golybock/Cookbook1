namespace Cookbook.Models.Database.Recipe;

public partial class RecipeCategory
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int CategoryId { get; set; }
}