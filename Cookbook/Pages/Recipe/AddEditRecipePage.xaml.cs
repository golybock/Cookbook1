using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Cookbook.Database.Services;
using Microsoft.Win32;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    private RecipeModel _recipe;
    private ClientModel _client;
    private RecipeService _recipeService;
    
    public AddEditRecipePage(ClientModel client)
    {
        _client = client;
        
        _recipe = new RecipeModel();
        _recipeService = new RecipeService(_client);

        InitializeComponent();
        
        DataContext = _recipe;
        // MediumPreview.DataContext = _recipe;
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        _recipe = recipe;
        _client = client;
        
        _recipe = new RecipeModel();
        _recipeService = new RecipeService(_client);
        
        InitializeComponent();
        
        DataContext = _recipe;
        // MediumPreview.DataContext = _recipe;
    }


}