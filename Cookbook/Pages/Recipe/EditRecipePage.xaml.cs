using System.Windows.Controls;
using Cookbook.ViewModels.Recipe;
using Models.Models.Database.Client;
using RecipeModel =  Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class EditRecipePage : Page
{
    public EditRecipePage(RecipeModel recipe, Client client)
    {
        InitializeComponent();
        EditRecipeView.DataContext = new EditRecipeViewModel(recipe, client);
    }
    
    public EditRecipePage(Client client)
    {
        InitializeComponent();
        EditRecipeView.DataContext = new EditRecipeViewModel(client);
    }
}