using Cookbook.ViewModels.Recipe;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    private EditRecipeViewModel _editRecipeViewModel;

    public AddEditRecipePage(ClientModel client)
    {
        var recipe = new RecipeModel();
        
        _editRecipeViewModel = new EditRecipeViewModel(recipe, client);

        _editRecipeViewModel.GetAll();
        
        InitializeComponent();
        
        DataContext = _editRecipeViewModel;
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        _editRecipeViewModel = new EditRecipeViewModel(recipe, client);
        
        _editRecipeViewModel.GetAll();
        
        InitializeComponent();
        
        DataContext = _editRecipeViewModel;
    }


}