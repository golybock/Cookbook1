using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Command;
using Cookbook.ContentDialogs;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe.Ingredients;
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
        new Category() { Id = -1, Name = "Выберите категорию" };

    private readonly RecipeType _defaultRecipeType =
        new RecipeType { Id = -1, Name = "Выберите тип" };

    private readonly Measure _defaultMeasure =
        new Measure() {Id = -1, Name = "Выберите меру измерения"};
    
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
        get => Categories
                   .FirstOrDefault(c => c.Id == Recipe.Category.Id) ??
               Categories.ElementAt(0);
        set
        {
            Recipe.Category = value ?? Categories.ElementAt(0);
            
            OnPropertyChanged();
        }
    }

    public RecipeType SelectedRecipeType
    {
        get => RecipeTypes
            .FirstOrDefault(c => c.Id == Recipe.RecipeType.Id) ??
               RecipeTypes.ElementAt(0);
        set
        {
            Recipe.RecipeType = value ?? RecipeTypes.ElementAt(0);
            
            OnPropertyChanged();
        } 
    }

    public bool ValidSelectedIngredient
    {
        get => _validSelectedIngredient;
        set
        {
            if (value == _validSelectedIngredient) return;
            _validSelectedIngredient = value;
            OnPropertyChanged();
        }
    }

    private RecipeIngredient RecipeIngredient { get; set; }

    public decimal RecipeIngredientCount
    {
        get => RecipeIngredient.Count;
        set
        {
            if (value == RecipeIngredient.Count) return;
            RecipeIngredient.Count = value;
            OnPropertyChanged();
        }
    }

    public RecipeModel Recipe { get; set; }

    public ObservableCollection<RecipeIngredient> RecipeIngredients
    {
        get => new(Recipe.RecipeIngredients);
        set
        {
            Recipe.RecipeIngredients = new(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Ingredient> Ingredients
    {
        get => new(_ingredients);
        set
        {
            _ingredients = new List<Ingredient>(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Category> Categories
    {
        get => new(_categories);
        set
        {
            _categories = new(value);
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<RecipeType> RecipeTypes
    {
        get => new(_recipeTypes);
        set
        {
            _recipeTypes = new(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Measure> Measures
    {
        get => new(_measures);
        set
        {
            _measures = new(value);
            OnPropertyChanged();
        }
    }

    private List<Measure> _measures;
    private List<Ingredient> _ingredients;
    private List<Category> _categories;
    private List<RecipeType> _recipeTypes;

    private Frame _navFrame;
    
    private readonly RecipeService _recipeService;
    private bool _validSelectedIngredient = true;

    public EditRecipeViewModel(RecipeModel recipe, ClientModel client, Frame frame)  
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        _measures = new List<Measure> {_defaultMeasure};
        
        _recipeService = new RecipeService(client);
        
        LoadComboboxes();
        
        Recipe = recipe;

        _navFrame = frame;
        
        Recipe.NewImagePath =
            Recipe.RecipeImage.ImagePath;

        SelectedIngredient = Ingredients.ElementAt(0); 
        
        RecipeIngredient = new RecipeIngredient(){Ingredient = Ingredients.ElementAt(0)};
    }

    public EditRecipeViewModel(ClientModel client, Frame frame)
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        _measures = new List<Measure> {_defaultMeasure};
        
        _recipeService = new RecipeService(client);
        
        LoadComboboxes();
        
        Recipe = new RecipeModel { Id = -1 };

        SelectedIngredient = Ingredients.ElementAt(0); 
        
        RecipeIngredient = new RecipeIngredient(){Ingredient = Ingredients.ElementAt(0)};
        
        _recipeService = new RecipeService(client);

        _navFrame = frame;
    }
    
    // команды для биндингов 
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
    
    
    private void OnEditImage() =>
        ChooseImage();
    
    private void OnAddCategory() =>
        ShowAddRecipeCategoryDialog();

    private void OnAddRecipeType() =>
        ShowAddRecipeTypeDialog();

    private void OnNewIngredient() =>
        ShowAddIngredientDialog();

    private void OnCancel() =>
        ShowCancelDialog();

    private void OnClear() =>
        ShowClearDialog();

    private void OnSave()
    {
        if(Recipe.Id == -1)
            CreateRecipe();
        
        else
            UpdateRecipe();
    }

    private void OnAddIngredient()
    {
        ValidSelectedIngredient = true;
        
        if (SelectedIngredient.Id != -1)
        {
            if (RecipeIngredient.Count > 0)
            {
                Recipe.RecipeIngredients.Add(RecipeIngredient);
                OnPropertyChanged("RecipeIngredients");
                RecipeIngredient = new RecipeIngredient();
                return;
            }
        }

        ValidSelectedIngredient = false;
    }
    
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
        Recipe.RecipeIngredients
            .Remove(
                Recipe
                    .RecipeIngredients
                    .FirstOrDefault(c => c.Id == id)!
            );
        RecipeIngredients = new(Recipe.RecipeIngredients);
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

    private async void UpdateRecipe()
    {
        
    }

    private async void LoadComboboxes()
    {
        await GetAll();
        SetDefaultSelectedValues();
    }
    
    private void SetDefaultSelectedValues()
    {
        SelectedCategory =
            Categories.FirstOrDefault(c => c.Id == Recipe.Category!.Id)!;

        SelectedRecipeType =
            RecipeTypes.FirstOrDefault(c => c.Id == Recipe.RecipeType!.Id)!;
    }
    
    private async Task GetAll()
    {
        _categories.AddRange(await GetCategories());
        OnPropertyChanged("Categories");
        
        _ingredients.AddRange(await GetIngredients());
        OnPropertyChanged("Ingredients");
        
        _recipeTypes.AddRange(await GetRecipeTypes());
        OnPropertyChanged("RecipeTypes");
        
        _measures.AddRange(await GetMeasures());
        OnPropertyChanged("Measures");
    }

    private async Task<List<Category>> GetCategories() =>
        await _recipeService.GetCategoriesAsync();

    private async Task<List<Ingredient>> GetIngredients() =>
        await _recipeService.GetIngredientsAsync();

    private async Task<List<RecipeType>> GetRecipeTypes() => 
        await _recipeService.GetRecipeTypes();

    private async Task<List<Measure>> GetMeasures() =>
        await _recipeService.GetMeasures();
    
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
                _categories.Add(category);
                Categories = new(_categories);
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
                _recipeTypes.Add(recipeType);
                RecipeTypes = new(_recipeTypes);
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
            Content = new AddIngredientView(ingredient,  new(Measures)),
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
                _ingredients.Add(ingredient);
                Ingredients = new(_ingredients);
            }
            
        }
    }
    
    private async void ShowCancelDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Отмена регистрации",
            Content = "Вы уверены, что хотите отменить регистрацию?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
            _navFrame
                .NavigationService?
                .GoBack();
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


