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
using Cookbook.Models.Database.Recipe;
using Cookbook.Models.Database.Recipe.Ingredients;
using Microsoft.Win32;
using ModernWpf.Controls;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Recipe;

public class EditRecipeViewModel : INotifyPropertyChanged
{
    private readonly Category _defaultCategory = new() {Id = -1, Name = "Выберите категорию"};

    private readonly Ingredient _defaultIngredient = new() {Id = -1, Name = "Выберите ингридиент"};

    private readonly Measure _defaultMeasure = new() {Id = -1, Name = "Выберите меру измерения"};

    private readonly RecipeType _defaultRecipeType = new() {Id = -1, Name = "Выберите тип"};

    private readonly RecipeService _recipeService;
    private List<Category> _categories;
    private List<Ingredient> _ingredients;

    private List<Measure> _measures;

    private readonly Frame _navFrame;
    private List<RecipeType> _recipeTypes;
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

        RecipeIngredient = new RecipeIngredient();

        SelectedIngredient = Ingredients.ElementAt(0);
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

        Recipe = new RecipeModel(){Id = -1, ClientId = client.Id};

        _navFrame = frame;

        Recipe.NewImagePath =
            Recipe.RecipeImage.ImagePath;

        RecipeIngredient = new RecipeIngredient();

        SelectedIngredient = Ingredients.ElementAt(0);
    }

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

    private RecipeIngredient RecipeIngredient { get; set; }

    public string RecipeText
    {
        get => Recipe.Text;
        set
        {
            if (value == Recipe.Text) return;
            Recipe.Text = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<RecipeIngredient> RecipeIngredients
    {
        get => new(Recipe.RecipeIngredients);
        set
        {
            Recipe.RecipeIngredients = new List<RecipeIngredient>(value);
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
            _categories = new List<Category>(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<RecipeType> RecipeTypes
    {
        get => new(_recipeTypes);
        set
        {
            _recipeTypes = new List<RecipeType>(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Measure> Measures
    {
        get => new(_measures);
        set
        {
            _measures = new List<Measure>(value);
            OnPropertyChanged();
        }
    }

    // команды для биндингов 
    public RelayCommand<DragEventArgs> DropCommand => new(OnDrop);

    public RelayCommand<int> RemoveIngredientFromListCommand => new(OnRemoveIngredientFromList);

    public CommandHandler EditImageCommand => new(OnEditImage);

    public CommandHandler AddIngredientCommand => new(OnAddIngredient);

    public CommandHandler ClearIngredientsCommand => new(OnClearIngredients);

    public CommandHandler AddCategoryCommand => new(OnAddCategory);

    public CommandHandler AddRecipeTypeCommand => new(OnAddRecipeType);

    public CommandHandler NewIngredientCommand => new(OnNewIngredient);

    public CommandHandler CancelCommand => new(OnCancel);

    public CommandHandler SaveCommand => new(OnSave);

    private void OnEditImage()
    {
        ChooseImage();
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
        ShowCancelDialog();
    }

    private void OnClearIngredients()
    {
        RecipeIngredients = new ObservableCollection<RecipeIngredient>();
    }

    private void OnSave()
    {
        if (Recipe.Id == -1)
            CreateRecipe();

        else
            UpdateRecipe();
    }

    private void OnAddIngredient()
    {
        ValidSelectedIngredient = true;

        if (SelectedIngredient.Id != -1)
            if (RecipeIngredient.Count > 0)
            {
                var existedIngredient = RecipeIngredients.FirstOrDefault(c => c.IngredientId == SelectedIngredient.Id);

                if (existedIngredient != null)
                {
                    RecipeIngredients
                            .FirstOrDefault(
                                c => c.IngredientId == SelectedIngredient.Id
                                )!
                            .Count
                        += RecipeIngredient.Count;
                }
                else
                {
                    Recipe.RecipeIngredients.Add(RecipeIngredient);
                }
                
                RecipeIngredient = new RecipeIngredient() { Count = RecipeIngredientCount};

                SelectedIngredient = Ingredients.ElementAt(0);

                OnPropertyChanged("RecipeIngredients");
                return;
            }

        ValidSelectedIngredient = false;
    }

    private void OnDrop(DragEventArgs obj)
    {
        var files = (string[]) obj.Data.GetData(DataFormats.FileDrop);
        var file = files[0];

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
                    .FirstOrDefault(c => c.IngredientId == id)!
            );

        RecipeIngredients = new ObservableCollection<RecipeIngredient>(Recipe.RecipeIngredients);
    }

    private async void CreateRecipe()
    {
        var createResult = await _recipeService.AddRecipeAsync(Recipe);

        if (createResult.Code != 100)
        {
            ShowErrorDialog(createResult.Description!);
        }

        else
        {
            ShowSavedDialog();
            _navFrame.NavigationService.GoBack();
        }
    }

    private async void UpdateRecipe()
    {
        var updateResult = await _recipeService.UpdateRecipeAsync(Recipe);

        if (updateResult.Code != 100)
        {
            ShowErrorDialog(updateResult.Description!);
        }

        else
        {
            ShowSavedDialog();
            _navFrame.NavigationService.GoBack();
        }
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

    private async void ShowAddRecipeCategoryDialog()
    {
        var category = new Category();

        var addDialog = new ContentDialog
        {
            Title = "Добавление категории рецепта",
            Content = new AddRecipeCategoryView(category),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            if (!string.IsNullOrWhiteSpace(category.Name))
            {
                var commandResult = await _recipeService.AddCategoryAsync(category);
                _categories.Add(category);
                Categories = new ObservableCollection<Category>(_categories);
            }
    }

    private async void ShowAddRecipeTypeDialog()
    {
        var recipeType = new RecipeType();

        var addDialog = new ContentDialog
        {
            Title = "Добавление типа рецепта",
            Content = new AddRecipeTypeView(recipeType),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            if (!string.IsNullOrWhiteSpace(recipeType.Name))
            {
                var commandResult = await _recipeService.AddRecipeTypeAsync(recipeType);
                _recipeTypes.Add(recipeType);
                RecipeTypes = new ObservableCollection<RecipeType>(_recipeTypes);
            }
    }

    private async void ShowSavedDialog()
    {
        var addDialog = new ContentDialog
        {
            Title = "Информация",
            Content = "Сохранено!",
            CloseButtonText = "Закрыть"
        };

        await addDialog.ShowAsync();
    }

    private async void ShowErrorDialog(string error)
    {
        var addDialog = new ContentDialog
        {
            Title = "Ошибка",
            Content = $"Введенные данные не верны: {error}",
            CloseButtonText = "Закрыть"
        };

        await addDialog.ShowAsync();
    }

    private void SetImage(string path) =>
        Recipe.NewImagePath = path;

    private void ChooseImage()
    {
        // открываем диалог выбора файла
        var openFileDialog = new OpenFileDialog();
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
        var ingredient = new Ingredient();

        var addDialog = new ContentDialog
        {
            Title = "Создание ингридиента",
            Content = new AddIngredientView(ingredient, new List<Measure>(Measures)),
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Добавить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await addDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            if (!string.IsNullOrWhiteSpace(ingredient.Name) && ingredient.Measure != null)
            {
                var commandResult = await _recipeService.AddIngredient(ingredient);
                _ingredients.Add(ingredient);
                Ingredients = new ObservableCollection<Ingredient>(_ingredients);
            }
    }

    private async void ShowCancelDialog()
    {
        var acceptDialog = new ContentDialog
        {
            Title = "Отмена регистрации",
            Content = "Вы уверены, что хотите отменить регистрацию?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await acceptDialog.ShowAsync();

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