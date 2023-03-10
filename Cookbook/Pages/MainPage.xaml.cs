using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Recipe;
using Frame = ModernWpf.Controls.Frame;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    public MainPage(Client client, Frame frame)
    {
        InitializeComponent();
        RecipesListView.DataContext = new RecipeListViewModel(client, frame);
    }
}