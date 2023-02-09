using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cookbook.Database.Services.Recipe;
using ModernWpf.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipesListView : UserControl
{
    private readonly RecipeService _recipeService;

    public RecipesListView()
    {
        _recipeService = new RecipeService();
        InitializeComponent();
    }
    
    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedItem = RecipesListBox.SelectedItem;
        
        if (selectedItem != null)
        {
            ShowAcceptDialog();
        }
    }

    private async void ShowAcceptDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Удаление элемента",
            Content = "Вы уверены, что хотите удалить этот рецепт?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Удалить",
            DefaultButton = ContentDialogButton.Primary
        };

        if (await acceptDialog.ShowAsync() == ContentDialogResult.Primary)
        {
            DeleteRecipe();
        }
    }

    private void DeleteRecipe()
    {
        var selectedItem = RecipesListBox.SelectedItem;

        var itemsSource = ItemsControl.ItemsControlFromItemContainer(RecipesListBox).ItemsSource;

        if (selectedItem is RecipeModel recipe)
            _recipeService.DeleteRecipeAsync(recipe.Id);
    }
    
    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        
    }

    private void RecipeMediumView_OnLikeClicked()
    {
        MessageBox.Show("Aboba");
    }
}