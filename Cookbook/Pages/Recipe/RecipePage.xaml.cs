using Cookbook.ViewModels.Recipe;
using Models.Models.Database.Client;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    public RecipePage(RecipeModel recipe, Client client, Frame frame)
    {
        InitializeComponent();
        
        DataContext = new RecipeMainViewModel(recipe, client, frame);
    }

}