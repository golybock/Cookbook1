using System.Windows;
using System.Windows.Controls;
using Models.Models.Database.Recipe;

namespace Cookbook.Views.Recipe;

public partial class EditRecipeView : UserControl
{
    public delegate void RemoveIngredientFromList(int id);
    public delegate void AddRecipeType();
    public delegate void AddCategory();
    public delegate void NewIngredient();
    public delegate void Clear();
    public delegate void Save();
    public delegate void Cancel();

    public event RemoveIngredientFromList? RemoveIngredientFromListClicked;
    public event AddRecipeType? AddRecipeTypeClicked;
    public event AddCategory? AddCategoryClicked;
    public event NewIngredient? NewIngredientClicked;
    public event Clear? ClearClicked;
    public event Save? SaveClicked;
    public event Cancel? CancelClicked;

    public EditRecipeView()
    {
        InitializeComponent();
    }

    private void RemoveIngredientFromListButton_OnClick(object sender, RoutedEventArgs e)
    {
        if(IngredientsListview.SelectedItem is RecipeIngredient selectedItem)
            RemoveIngredientFromListClicked?.Invoke(selectedItem.Id);
    }

    private void AddRecipeTypeButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddRecipeTypeClicked?.Invoke();
    }

    private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddCategoryClicked?.Invoke();
    }

    private void NewIngredientButton_OnClick(object sender, RoutedEventArgs e)
    {
        NewIngredientClicked?.Invoke();
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClearClicked?.Invoke();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        SaveClicked?.Invoke();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        CancelClicked?.Invoke();
    }
}