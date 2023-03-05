using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.ContentDialogs;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe.Ingredients;
using Cookbook.Views.Recipe;
using Microsoft.Win32;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;
using ModernWpf.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;


namespace Cookbook.ViewModels.Recipe;

public class EditRecipeViewModel : INotifyPropertyChanged
{
    private readonly Ingredient _defaultIngredient =
        new Ingredient { Id = -1, Name = "Выберите ингридиент" };

    private readonly Category _defaultCategory =
        new Category() {Id = -1, Name = "Выберите категорию"};

    private readonly RecipeType _defaultRecipeType =
        new RecipeType { Id = -1, Name = "Выберите тип" };


    public Ingredient SelectedIngredient
    {
        get => RecipeIngredient.Ingredient ?? Ingredients.ElementAt(0);
        set
        {
            RecipeIngredient.Ingredient = value;
            OnPropertyChanged();
        } 
    }

    public Category SelectedCategory
    {
        get => Recipe.Category ?? Categories.ElementAt(0);
        set
        {
            Recipe.Category =
                Categories.FirstOrDefault(c => c.Id == value.Id);
            
            OnPropertyChanged();
        }
    }

    public RecipeType SelectedRecipeType
    {
        get => Recipe.RecipeType ?? RecipeTypes.ElementAt(0);
        set
        {
            Recipe.RecipeType =
                RecipeTypes.FirstOrDefault(c => c.Id == value.Id)!;
            
            OnPropertyChanged();
        } 
    }
    
    private RecipeIngredient RecipeIngredient { get; set; }
    public RecipeModel Recipe { get; set; }

    public List<Ingredient> Ingredients
    {
        get => _ingredients;
        set
        {
            _ingredients = value;
            OnPropertyChanged();
        }
    }

    public List<Category> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged();
        }
    }
    
    public List<RecipeType> RecipeTypes
    {
        get => _recipeTypes;
        set
        {
            _recipeTypes = value;
            OnPropertyChanged();
        }
    }

    public List<Measure> Measures
    {
        get => _measures;
        set
        {
            _measures = value;
            OnPropertyChanged();
        }
    }

    private List<Measure> _measures;
    private List<Ingredient> _ingredients;
    private List<Category> _categories;
    private List<RecipeType> _recipeTypes;

    private Frame _navFrame;
    
    private readonly RecipeService _recipeService;

    public EditRecipeViewModel(RecipeModel recipe, ClientModel client, Frame frame)  
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        
        Recipe = recipe;

        _navFrame = frame;
        
        Recipe.NewImagePath =
            Recipe.RecipeImage.ImagePath;
        
        Recipe.Category =
            Categories.FirstOrDefault(c => c.Id == Recipe.Category!.Id);
        
        Recipe.RecipeType =
            RecipeTypes.FirstOrDefault(c => c.Id == Recipe.RecipeType.Id)!;

        SelectedIngredient = Ingredients.ElementAt(0); 
        
        RecipeIngredient = new RecipeIngredient(){Ingredient = Ingredients.ElementAt(0)};
        
        _recipeService = new RecipeService(client);
    }

    public EditRecipeViewModel(ClientModel client, Frame frame)
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        
        Recipe = new RecipeModel();
        
        Recipe.Category =
            Categories.FirstOrDefault(c => c.Id == Recipe.Category!.Id);
        
        Recipe.RecipeType =
            RecipeTypes.FirstOrDefault(c => c.Id == Recipe.RecipeType!.Id);

        SelectedIngredient = Ingredients.ElementAt(0); 
        
        RecipeIngredient = new RecipeIngredient(){Ingredient = Ingredients.ElementAt(0)};
        
        _recipeService = new RecipeService(client);

        _navFrame = frame;
    }
    
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);
    
    public RelayCommand<int> RemoveIngredientFromListCommand =>
        new RelayCommand<int>(OnRemoveIngredientFromList);

    public CommandHandler EditImageCommand =>
        new CommandHandler(OnEditImage);

    public CommandHandler AddIngredientCommand =>
        new CommandHandler(OnAddIngredient);

    public CommandHandler AddCategoryCommand =>
        new CommandHandler(OnAddCategory);

    public CommandHandler AddRecipeTypeCommand =>
        new CommandHandler(OnAddRecipeType);

    public CommandHandler NewIngredientCommand =>
        new CommandHandler(OnNewIngredient);

    public CommandHandler CancelCommand =>
        new CommandHandler(OnCancel);

    public CommandHandler SaveCommand =>
        new CommandHandler(OnSave);

    public CommandHandler ClearCommand =>
        new CommandHandler(OnClear);

    private void OnDrop(DragEventArgs obj)
    {
        string[] files = (string[]) obj.Data.GetData(DataFormats.FileDrop);
        string file = files[0];
        
        // если файл картинка
        if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            SetImage(file);
    }

    private void OnRemoveIngredientFromList(int id)
    {
        throw new System.NotImplementedException();
    }

    private void OnEditImage() =>
        ChooseImage();

    private void OnAddIngredient()
    {
        throw new System.NotImplementedException();
    }
    
    private void OnAddCategory()
    {
        ShowAddRecipeCategoryDialog();
    }
    
    private void OnAddRecipeType()
    {
        ShowAddRecipeTypeDialog();
    }
    
    private void OnNewIngredient()
    {
        ShowAddIngredientDialog();
    }
    
    private void OnCancel()
    {
        throw new System.NotImplementedException();
    }
    
    private void OnSave()
    {

    }

    private void OnClear() =>
        ShowClearDialog();

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
    
    private async void GetAll()
    {
        Categories = await GetCategories();
        Ingredients = await GetIngredients();
        RecipeTypes = await GetRecipeTypes();
    }

    private async Task<List<Category>> GetCategories() =>
        await _recipeService.GetCategoriesAsync();

    private async Task<List<Ingredient>> GetIngredients() =>
        await _recipeService.GetIngredientsAsync();

    private async Task<List<RecipeType>> GetRecipeTypes() => 
        await _recipeService.GetRecipeTypes();

    private void ClearPage() =>
        Recipe = new RecipeModel();

    private async void ShowClearDialog()
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
                Categories = Categories;
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
            }
            
        }
    }
    
    private void SetImage(string path) => 
        Recipe.NewImagePath = path;

    private void ChooseImage()
    {
        // открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
        // если показан
        if (openFileDialog.ShowDialog() == true)
            // если есть выбранный файл
            if (openFileDialog.FileName != string.Empty)
                SetImage(openFileDialog.FileName);
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
                Ingredients = Ingredients;
            }
            
        }
    }
    
    // для привязки данных, реализация InotifyPropertyChanged
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


