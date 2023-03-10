using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Recipe;
using Frame = ModernWpf.Controls.Frame;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class EditRecipePage : Page
{
    public EditRecipePage(RecipeModel recipe, Client client, Frame frame)
    {
        InitializeComponent();
        EditRecipeView.DataContext = new EditRecipeViewModel(recipe, client, frame);
    }

    public EditRecipePage(Client client, Frame frame)
    {
        InitializeComponent();
        EditRecipeView.DataContext = new EditRecipeViewModel(client, frame);
    }
}