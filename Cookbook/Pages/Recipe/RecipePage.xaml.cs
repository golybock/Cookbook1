using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Recipe;
using Frame = ModernWpf.Controls.Frame;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    public RecipePage(RecipeModel recipe, Client client, Frame frame)
    {
        InitializeComponent();

        DataContext = new RecipeMainViewModel(recipe, client, frame);
    }
}