using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

public partial class AddEditRecipePage : Page
{
    public RecipeIngredient RecipeIngredient { get; set; }
    public RecipeModel Recipe { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Measure> Measures { get; set; }
    public List<Category> Categories { get; set; }
    public List<RecipeType> RecipeTypes { get; set; }

    private readonly RecipeService _recipeService;
    

    public AddEditRecipePage(ClientModel client)
    {
        Measures = new List<Measure>();
        RecipeIngredient = new RecipeIngredient();
        Categories = new List<Category>();
        RecipeTypes = new List<RecipeType>();
        Ingredients = new List<Ingredient>();
        Recipe = new RecipeModel();

        Recipe = new RecipeModel();
        Recipe.ClientId = client.Id;
        
        _recipeService = new RecipeService(client);
        
        InitializeComponent();
        
        GetAll();
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        Measures = new List<Measure>();
        RecipeIngredient = new RecipeIngredient();
        Categories = new List<Category>();
        RecipeTypes = new List<RecipeType>();
        Ingredients = new List<Ingredient>();
        Recipe = new RecipeModel();
        
        _recipeService = new RecipeService(client);
        
        InitializeComponent();
        
        GetAll();
    }

    private async void GetAll()
    {
        Measures = new List<Measure>() {new Measure() {Id = -1, Name = "Не выбрано"}};
        Categories = new List<Category>() {new Category() {Id = -1, Name = "Выберите категорию"}};
        RecipeTypes = new List<RecipeType>() {new RecipeType() { Id = -1, Name = "Выберите тип" }};
        Ingredients = new List<Ingredient>(){new Ingredient() { Id = -1, Name = "Выберите ингридиент"}};
        
        Categories.AddRange(await GetCategories());
        Ingredients.AddRange(await GetIngredients());
        RecipeTypes.AddRange(await GetRecipeTypes());
        Measures.AddRange(await GetMeasures());
        
        DataContext = this;
    }

    private async Task<List<Measure>> GetMeasures()
    {
        return await _recipeService.GetMeasures();
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

    private async void ShowAddRecipeCategoryDialog()
    {
        Category category = new Category();
        
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Добавление категории рецепта",
            Content = new AddRecipeCategoryView(category),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (!string.IsNullOrWhiteSpace(category.Name))
            {
                var commandResult = await _recipeService.AddRecipeCategoryAsync(category);
                Categories.Add(category);
                EditRecipeView.CategoryComboBox.ItemsSource = null;
                EditRecipeView.CategoryComboBox.ItemsSource = Categories;
                EditRecipeView.CategoryComboBox.SelectedIndex = 0;
            }
            
        }
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
                RecipeTypes.Add(recipeType);
                EditRecipeView.RecipeTypeComboBox.ItemsSource = null;
                EditRecipeView.RecipeTypeComboBox.ItemsSource = RecipeTypes;
                EditRecipeView.RecipeTypeComboBox.SelectedIndex = 0;
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
    

    private void EditRecipeView_OnAddRecipeTypeClicked()
    {
        CreateRecipeType();
    }

    private void EditRecipeView_OnAddCategoryClicked()
    {
        CreateRecipeCategory();
    }

    private void EditRecipeView_OnNewIngredientClicked()
    {
        CreateIngredient();
    }

    private void EditRecipeView_OnClearClicked()
    {
        ShowAcceptDialog();
    }

    private void EditRecipeView_OnSaveClicked()
    {
        CreateRecipe();
    }

    private async void CreateRecipe()
    {
        var createResult = await _recipeService.AddRecipeAsync(Recipe);

        if (createResult.Code != 100)
        {
            MessageBox.Show("Ошибка");
        }
        else
        {
            MessageBox.Show("ес");
        }
    }
    
    private void EditRecipeView_OnCancelClicked()
    {
        ShowAcceptDialogToCancel();
    }

    private async void ShowAcceptDialogToCancel()
    {
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Подтверждение",
            Content = "Отменить добавление рецепта?",
            CloseButtonText = "Нет, продолжить",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (NavigationService != null) 
                NavigationService.GoBack();
        }

    }

    private async void ShowAddIngredientDialog()
    {
        Ingredient ingredient = new Ingredient();
        
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Создание ингридиента",
            Content = new AddIngredientView(ingredient, Measures),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (!string.IsNullOrWhiteSpace(ingredient.Name) && ingredient.Measure != null)
            {
                var commandResult = await _recipeService.AddIngredient(ingredient);
                Ingredients.Add(ingredient);
                EditRecipeView.IngredientsComboBox.ItemsSource = null;
                EditRecipeView.IngredientsComboBox.ItemsSource = Ingredients;
                EditRecipeView.IngredientsComboBox.SelectedIndex = 0;
            }
            
        }
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
        ShowAddRecipeTypeDialog();
    }

    private void CreateRecipeCategory()
    {
        ShowAddRecipeCategoryDialog();
    }

    private void CreateIngredient()
    {
        ShowAddIngredientDialog();
    }

}