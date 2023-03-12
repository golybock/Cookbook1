using System.Collections.Generic;

namespace Cookbook.Models.Database.Recipe;

public partial class Recipe
{
    private string? _pathToTextFile;
    private List<RecipeIngredient> _recipeIngredients = new();

    public Recipe()
    {
        RecipeImage = new RecipeImage {RecipeId = Id};
    }


    public int Id { get; set; }

    public int ClientId { get; set; }

    public int RecipeTypeId => RecipeType.Id;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? PathToTextFile
    {
        get => _pathToTextFile;
        set
        {
            if (value == _pathToTextFile) return;
            _pathToTextFile = value;
            Text = GetText();
            OnPropertyChanged();
        }
    }

    public int PortionCount { get; set; }

    public int CookingTime { get; set; }

    public List<RecipeIngredient> RecipeIngredients
    {
        get => _recipeIngredients;
        set
        {
            if (Equals(value, _recipeIngredients)) return;
            _recipeIngredients = value;
            OnPropertyChanged();
        }
    }

    public RecipeStats RecipeStat { get; set; } = new();

    public RecipeType RecipeType { get; set; } = new();
}