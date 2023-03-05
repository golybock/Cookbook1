using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Command;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;

namespace Cookbook.Views.Recipe;

public partial class EditRecipeView : UserControl
{
    
    public static readonly DependencyProperty RemoveIngredientFromListProperty =
        DependencyProperty.Register(
            "RemoveIngredientFromList",
            typeof(RelayCommand<int>),
            typeof(UserControl));
    
    public static readonly DependencyProperty AddRecipeTypeProperty =
        DependencyProperty.Register(
            "AddRecipeType",
            typeof(CommandHandler),
            typeof(UserControl));

    public static readonly DependencyProperty AddCategoryProperty =
        DependencyProperty.Register(
            "AddCategory",
            typeof(CommandHandler),
            typeof(UserControl));
    
    public static readonly DependencyProperty ClearProperty =
        DependencyProperty.Register(
            "Clear",
            typeof(CommandHandler),
            typeof(UserControl));
    
    public static readonly DependencyProperty SaveProperty =
        DependencyProperty.Register(
            "Save",
            typeof(CommandHandler),
            typeof(UserControl));
    
    public static readonly DependencyProperty CancelProperty =
        DependencyProperty.Register(
            "Cancel",
            typeof(CommandHandler),
            typeof(UserControl));

    public static readonly DependencyProperty ChooseImageProperty =
        DependencyProperty.Register(
            "ChooseImage",
            typeof(CommandHandler),
            typeof(UserControl));
    
    public static readonly DependencyProperty ImageDropProperty =
        DependencyProperty.Register(
            "ImageDrop",
            typeof(RelayCommand<int>),
            typeof(UserControl));

    public RelayCommand<int> RemoveIngredientFromList
    {
        get => (RelayCommand<int>) GetValue(RemoveIngredientFromListProperty);
        set => SetValue(RemoveIngredientFromListProperty, value);
    }

    public CommandHandler AddRecipeType
    {
        get => (CommandHandler) GetValue(AddRecipeTypeProperty);
        set => SetValue(AddRecipeTypeProperty, value);
    }

    public CommandHandler AddCategory
    {
        get => (CommandHandler) GetValue(AddCategoryProperty);
        set => SetValue(AddCategoryProperty, value);
    }

    public CommandHandler Clear
    {
        get => (CommandHandler) GetValue(ClearProperty);
        set => SetValue(ClearProperty, value);
    }

    public CommandHandler Save
    {
        get => (CommandHandler) GetValue(SaveProperty);
        set => SetValue(SaveProperty, value);
    }

    public CommandHandler Cancel
    {
        get => (CommandHandler) GetValue(CancelProperty);
        set => SetValue(CancelProperty, value);
    }

    public CommandHandler ChooseImage
    {
        get => (CommandHandler) GetValue(ChooseImageProperty);
        set => SetValue(ChooseImageProperty, value);
    }

    public RelayCommand<int> ImageDrop
    {
        get => (RelayCommand<int>) GetValue(ImageDropProperty);
        set => SetValue(ImageDropProperty, value);
    }

    public EditRecipeView()
    {
        InitializeComponent();
        
        CategoryComboBox.SelectedIndex = 0;
        RecipeTypeComboBox.SelectedIndex = 0;
    }

    private void RemoveIngredientFromListButton_OnClick(object sender, RoutedEventArgs e)
    {
        if(IngredientsListview.SelectedItem is RecipeIngredient selectedItem)
            RemoveIngredientFromList.Execute(selectedItem.Id);
    }

    private void AddRecipeTypeButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddRecipeType.Execute(null);
    }

    private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddCategory?.Execute(null);
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        Clear?.Execute(null);
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        Save.Execute(null);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Cancel.Execute(null);
    }

    private void RecipeImage_OnDrop(object sender, DragEventArgs e)
    {
        ImageDrop.Execute(e);
    }

    private void RecipeImage_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ChooseImage.Execute(null);
    }

}