using System.Windows.Controls;
using Cookbook.Database;

namespace Cookbook.Pages.RecipesPage;

public partial class RecipePage : Page
{
    private global::Models.Models.Database.Recipe.Recipe _recipe;
    
    public RecipePage(global::Models.Models.Database.Recipe.Recipe recipe)
    {
        _recipe = recipe;
        InitializeComponent();
        DataContext = _recipe;
    }
}