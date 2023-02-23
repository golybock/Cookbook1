using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;

namespace Cookbook.Views.Recipe;

public partial class EditRecipeView : UserControl
{
    public Category? SelectedCategory =>
        CategoryComboBox.SelectedItem as Category;
    public RecipeType? SelectedRecipeType =>
        RecipeTypeComboBox.SelectedItem as RecipeType;

    public Ingredient? SelectedIngredient =>
        IngredientsComboBox.SelectedItem as Ingredient;

    public delegate void RemoveIngredientFromList(int id);
    public delegate void AddRecipeType();
    public delegate void AddCategory();
    public delegate void NewIngredient();
    public delegate void Clear();
    public delegate void Save();
    public delegate void Cancel();
    public delegate void ChooseImage();
    public delegate void ImageDrop(DragEventArgs e);

    public event RemoveIngredientFromList? RemoveIngredientFromListClicked;
    public event AddRecipeType? AddRecipeTypeClicked;
    public event AddCategory? AddCategoryClicked;
    public event NewIngredient? NewIngredientClicked;
    public event Clear? ClearClicked;
    public event Save? SaveClicked;
    public event Cancel? CancelClicked;
    public event ChooseImage? ChooseImageCLicked;
    public event ImageDrop? ImageDropped;

    public EditRecipeView()
    {
        InitializeComponent();
        
        CategoryComboBox.SelectedIndex = 0;
        RecipeTypeComboBox.SelectedIndex = 0;
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

    private void RecipeImage_OnDrop(object sender, DragEventArgs e)
    {
        ImageDropped?.Invoke(e);
    }

    private void RecipeImage_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ChooseImageCLicked?.Invoke();
    }

}