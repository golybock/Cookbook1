namespace Models.Models.Database.Recipe;

public partial class Recipe
{
    public Recipe() =>
        RecipeImage = new RecipeImage() {RecipeId = Id};
    
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int RecipeTypeId { get; set; }
    
    public string Name { get; set; }

    public DateTime DateOfCreation { get; set; }

    public string? Description { get; set; }

    public string? PathToTextFile { get; set; }

    public int PortionCount { get; set; }

    public int CookingTime { get; set; }

    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();

    public List<RecipeImage> RecipeImages { get; set; } = new();

    public RecipeStats RecipeStat { get; set; } = new();

    public RecipeType? RecipeType { get; set; } = new();
    
}