using System.Windows.Controls;
using Models.Models.Database.Recipe;

namespace Cookbook.ContentDialogs;

public partial class AddRecipeTypeView : UserControl
{
    public AddRecipeTypeView(RecipeType recipeType)
    {
        DataContext = recipeType;
        InitializeComponent();
    }
}