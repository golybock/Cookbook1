using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Database.Services.Recipe;
using Cookbook.Pages.RecipesPage;

namespace Cookbook.Pages.Recipe;

public partial class RecipesPage : Page
{
    private readonly RecipeService _recipeService;

    public RecipesPage()
    {
        _recipeService = new RecipeService();
        InitializeComponent();
        LoadRecipes();
    }

    private async void LoadRecipes()
    {
        // RecipesGridView.ItemsSource = await _recipeService.GetRecipesAsync();
    }
    
    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var accept = 
            MessageBox.Show(
                "Удалить?",
                "Удаление",
                MessageBoxButton.YesNo);

        if (accept == MessageBoxResult.Yes)
        {
            // удалить
        }
        
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        // if (NavigationService != null)
        //     NavigationService.Navigate(
        //         new AddEditRecipePage(RecipesGridView.SelectedItem as Models.Database.Recipe.Recipe));
    }

    private void RecipesListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // if(RecipesGridView.SelectedItem != null)
        //     if (NavigationService != null)
        //         NavigationService.Navigate(
        //             new RecipePage(RecipesGridView.SelectedItem as Models.Database.Recipe.Recipe));
    }
}