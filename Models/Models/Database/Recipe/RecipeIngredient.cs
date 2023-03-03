using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database.Recipe.Ingredients;

namespace Models.Models.Database.Recipe;

public partial class RecipeIngredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Count { get; set; }
    
    public Ingredient? Ingredient { get; set; } = new Ingredient();
}