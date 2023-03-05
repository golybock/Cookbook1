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

    private List<Ingredient> _ingredients;
    private List<Category> _categories;
    private List<RecipeType> _recipeTypes;

    private readonly RecipeService _recipeService;

    public EditRecipeViewModel(RecipeModel recipe, ClientModel client)  
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        
        Recipe = recipe;

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

    public EditRecipeViewModel(ClientModel client)
    {
        RecipeIngredient = new RecipeIngredient();

        _ingredients = new List<Ingredient> {_defaultIngredient};
        _categories = new List<Category> {_defaultCategory};
        _recipeTypes = new List<RecipeType> {_defaultRecipeType};
        
        Recipe = new RecipeModel();
        
        Recipe.Category =
            Categories.FirstOrDefault(c => c.Id == Recipe.Category!.Id);
        
        Recipe.RecipeType =
            RecipeTypes.FirstOrDefault(c => c.Id == Recipe.RecipeType.Id)!;

        SelectedIngredient = Ingredients.ElementAt(0); 
        
        RecipeIngredient = new RecipeIngredient(){Ingredient = Ingredients.ElementAt(0)};
        
        _recipeService = new RecipeService(client);
    }
    
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);

    public RelayCommand<int> RemoveIngredientFromListCommand =>
        new RelayCommand<int>(OnRemoveIngredientFromList);

    public CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

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
    
     //     private void AddButton_OnClick(object sender, RoutedEventArgs e)
    // {
    //     AddRecipe();
    // }
    
    // private async Task AddRecipe()
    // {
    //     await _recipeService.AddRecipeAsync(_recipe);
    // }
    //
    // private void OutError(string error)
    // {
    //     ErrorTextBlock.Text = error;
    //     ErrorTextBlock.Visibility = Visibility.Visible;
    // }
    //
    // private void ClearError()
    // {
    //     ErrorTextBlock.Text = null;
    //     ErrorTextBlock.Visibility = Visibility.Collapsed;
    // }
    //
    //
    // private void Input(object sender, TextChangedEventArgs e)
    // {
    //     ClearError();
    // }
    //
    // private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    // {
    //     ShowAcceptDialog();
    // }
    //
    // private void ClearPage()
    // {
    //     _recipe = new RecipeModel();
    //     
    //     DataContext = null;
    //     
    //     DataContext = _recipe;
    // }
    //
    // private async void ShowAcceptDialog()
    // {
    //     ContentDialog acceptDialog = new ContentDialog()
    //     {
    //         Title = "Очистка ввода",
    //         Content = "Вы уверены, что хотите очистить все введенные данные?",
    //         CloseButtonText = "Отмена",
    //         PrimaryButtonText = "Очистить",
    //         DefaultButton = ContentDialogButton.Primary
    //     };
    //     
    //     ContentDialogResult result = await acceptDialog.ShowAsync();
    //
    //     if (result == ContentDialogResult.Primary)
    //         ClearPage();
    //     
    // }
    //
    // private void ImageView_OnDrop(object sender, DragEventArgs e)
    // {
    //     string[] files = ((string[]) e.Data.GetData(DataFormats.FileDrop));
    //     string file = files[0];
    //     // если файл картинка
    //     if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //         SetImage(file);
    // }
    //
    // private void SetImage(string path)
    // {
    //     // сохраняем путь в объекте
    //     _recipe.RecipeImage.ImagePath = path;
    //     _recipe.RecipeImages.Add(new (){ RecipeId = _recipe.Id, ImagePath = path});
    //     // отображаем изображение
    //     if (_recipe.RecipeImage.ImagePath != null)
    //         RecipeImage.Source = new BitmapImage(new Uri(_recipe.RecipeImage.ImagePath));
    // }
    //
    // private void ImageView_OnMouseDown(object sender, MouseButtonEventArgs e)
    // {
    //     ChooseImage();
    // }
    // private void ChooseImage()
    // {
    //     // открываем диалог выбора файла
    //     OpenFileDialog openFileDialog = new OpenFileDialog();
    //     openFileDialog.InitialDirectory = "c:\\";
    //     openFileDialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
    //     // если показан
    //     if (openFileDialog.ShowDialog() == true)
    //     {
    //         // если есть выбранный файл
    //         if (openFileDialog.FileName != String.Empty)
    //         {
    //             SetImage(openFileDialog.FileName);
    //         }
    //     }
    // }

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


