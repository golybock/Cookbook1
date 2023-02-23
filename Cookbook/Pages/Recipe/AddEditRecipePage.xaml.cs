using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Cookbook.Database.Services;
using Cookbook.ViewModels.Recipe;
using Cookbook.Views.Recipe;
using Microsoft.Win32;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    private EditRecipeViewModel _editRecipeViewModel;
    private RecipeService _recipeService;
    
    public AddEditRecipePage(ClientModel client)
    {
        var recipe = new RecipeModel();
        _recipeService = new RecipeService(client);
        _editRecipeViewModel = new EditRecipeViewModel(recipe, client);

        InitializeComponent();
        
        DataContext = _editRecipeViewModel;
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        _recipeService = new RecipeService(client);
        _editRecipeViewModel = new EditRecipeViewModel(recipe, client);
        
        InitializeComponent();
        
        DataContext = _editRecipeViewModel;
    }


}