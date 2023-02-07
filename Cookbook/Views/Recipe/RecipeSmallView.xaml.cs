using System.Windows.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeSmallView : UserControl
{
    public RecipeSmallView(RecipeModel recipe)
    {
        InitializeComponent();
        DataContext = recipe;
    }

    public RecipeSmallView()
    {
        InitializeComponent();
    }
}