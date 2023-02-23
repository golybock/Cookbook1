using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.ContentDialogs;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe.Ingredients;
using Microsoft.Win32;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page, INotifyPropertyChanged
{
    public RecipeIngredient RecipeIngredient { get; set; }
    public RecipeModel Recipe { get; set; }
    public ObservableCollection<Ingredient> Ingredients { get; set; }
    public ObservableCollection<Measure> Measures { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<RecipeType> RecipeTypes { get; set; }

    private readonly RecipeService _recipeService;
    

    public AddEditRecipePage(ClientModel client)
    {
        RecipeIngredient = new RecipeIngredient();

        Recipe = new RecipeModel();
        
        _recipeService = new RecipeService(client);
        
        InitializeComponent();
        
        GetAll();
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        Measures = new ObservableCollection<Measure>();
        RecipeIngredient = new RecipeIngredient();
        Categories = new ObservableCollection<Category>();
        RecipeTypes = new ObservableCollection<RecipeType>();
        Ingredients = new ObservableCollection<Ingredient>();
        Recipe = new RecipeModel();
        
        _recipeService = new RecipeService(client);
        
        InitializeComponent();
        
        GetAll();
    }

    private async void GetAll()
    {
        Measures = new ObservableCollection<Measure>() {new Measure() {Id = -1, Name = "Не выбрано"}};
        Categories = new ObservableCollection<Category>() {new Category() {Id = -1, Name = "Выберите категорию"}};
        RecipeTypes = new ObservableCollection<RecipeType>() {new RecipeType() { Id = -1, Name = "Выберите тип" }};
        Ingredients = new ObservableCollection<Ingredient>(){new Ingredient() { Id = -1, Name = "Выберите ингридиент"}};

        foreach (var category in await GetCategories())
            Categories.Add(category);

        foreach (var ingredient in await GetIngredients())
            Ingredients.Add(ingredient);

        foreach (var recipeType in await GetRecipeTypes())
            RecipeTypes.Add(recipeType);
        
        DataContext = this;
    }

    private async Task<List<Category>> GetCategories()
    {
        return await _recipeService.GetCategoriesAsync();
    } 
    
    private async Task<List<Ingredient>> GetIngredients()
    {
        return await _recipeService.GetIngredientsAsync();
    }

    private async Task<List<RecipeType>> GetRecipeTypes()
    {
        return await _recipeService.GetRecipeTypes();
    }

    private void ClearPage()
    {
        Recipe = new RecipeModel();
        
        DataContext = null;
        
        DataContext = this;
    }
    
    private async void ShowAcceptDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Очистка ввода",
            Content = "Вы уверены, что хотите очистить все введенные данные?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Очистить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
            ClearPage();
    }

    private async void ShowAddRecipeTypeDialog()
    {
        RecipeType recipeType = new RecipeType();
        
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Добавление типа рецепта",
            Content = new AddRecipeTypeView(recipeType),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (!string.IsNullOrWhiteSpace(recipeType.Name))
            {
                var commandResult = await _recipeService.AddRecipeTypeAsync(recipeType);
                recipeType.Id = commandResult.ValueId;
                RecipeTypes.Add(recipeType);
            }
            
        }
    }
    
    private void SetImage(string path)
    {
        // сохраняем путь в объекте
        Recipe.RecipeImage.ImagePath = path;
        Recipe.RecipeImages.Add(new (){ RecipeId = Recipe.Id, ImagePath = path});
    }

    private void ChooseImage()
    {
        // открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
        // если показан
        if (openFileDialog.ShowDialog() == true)
        {
            // если есть выбранный файл
            if (openFileDialog.FileName != String.Empty)
            {
                SetImage(openFileDialog.FileName);
            }
        }
    }

    private void AddEditRecipePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        GetAll();
    }

    private void EditRecipeView_OnRemoveIngredientFromListClicked(int id)
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnAddRecipeTypeClicked()
    {
        ShowAddRecipeTypeDialog();
    }

    private void EditRecipeView_OnAddCategoryClicked()
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnNewIngredientClicked()
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnClearClicked()
    {
        ShowAcceptDialog();
    }

    private void EditRecipeView_OnSaveClicked()
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnCancelClicked()
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnChooseImageCLicked()
    {
        ChooseImage();
    }

    private void EditRecipeView_OnImageDropped(DragEventArgs e)
    {
        string[] files = ((string[]) e.Data.GetData(DataFormats.FileDrop));
        string file = files[0];
        
        // если файл картинка
        if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            SetImage(file);
    }

    private void CreateRecipeType()
    {
        
        
        
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}