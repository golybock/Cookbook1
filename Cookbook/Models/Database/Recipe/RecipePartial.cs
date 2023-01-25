namespace Cookbook.Models.Database.Recipe;

public class RecipePartial
{
    public string? ImagePath
    {
        get => ImagePath ?? "../../Resources/not_found_image.png";
        set => ImagePath = value;
    }
}