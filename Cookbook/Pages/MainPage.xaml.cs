using Cookbook.ViewModels.Recipe;
using Models.Models.Database.Client;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages;

public partial class MainPage : Page
{
    public MainPage(Client client, Frame frame)
    {
        InitializeComponent();
        RecipesListView.DataContext = new RecipeListViewModel(client, frame);
    }
}