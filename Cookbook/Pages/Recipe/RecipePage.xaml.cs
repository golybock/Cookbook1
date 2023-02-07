using System.Windows.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    private readonly RecipeModel _recipe;
    
    public RecipePage(RecipeModel recipe)
    {
        _recipe = recipe;
        InitializeComponent();
        DataContext = _recipe;
    }
}