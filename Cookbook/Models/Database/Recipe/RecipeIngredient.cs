using Cookbook.Models.Database.Recipe.Ingredients;

namespace Cookbook.Models.Database.Recipe;

public partial class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Count { get; set; }
    
    public virtual Ingredient Ingredient { get; set; } = null!;
}