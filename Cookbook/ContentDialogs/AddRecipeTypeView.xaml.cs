using System.Windows.Controls;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.ContentDialogs;

public partial class AddRecipeTypeView : UserControl
{
    public AddRecipeTypeView(RecipeType recipeType)
    {
        DataContext = recipeType;
        InitializeComponent();
    }
}