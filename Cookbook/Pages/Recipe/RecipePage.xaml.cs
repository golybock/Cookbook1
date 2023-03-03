using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    public RecipePage(RecipeModel recipe)
    {
        DataContext = recipe;
        
        InitializeComponent();
    }

}