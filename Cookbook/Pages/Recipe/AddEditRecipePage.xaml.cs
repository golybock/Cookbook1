using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Recipe.Ingredients;
using Cookbook.ViewModels.Recipe;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;
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
        Measures = new List<Measure>() {new Measure() {Id = -1, Name = "Не выбрано"}};
        RecipeIngredient = new RecipeIngredient();
        Categories = new List<Category>() {new Category() {Id = -1, Name = "Выберите категорию"}};
        RecipeTypes = new List<RecipeType>() {new RecipeType() { Id = -1, Name = "Выберите тип" }};
        Ingredients = new List<Ingredient>(){new Ingredient() { Id = -1, Name = "Выберите ингридиент"}};
        
        Recipe = new RecipeModel();
        
        _recipeService = new RecipeService(client);
        
        InitializeComponent();
        
        GetAll();
        
        DataContext = this;
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
        
        DataContext = this;
    }

    private async void GetAll()
    {
        Categories.AddRange(await GetCategories());
        Ingredients.AddRange(await GetIngredients());
        RecipeTypes.AddRange(await GetRecipeTypes());
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

    private void AddEditRecipePage_OnLoaded(object sender, RoutedEventArgs e)
    {

    }

    private void EditRecipeView_OnRemoveIngredientFromListClicked(int id)
    {
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnAddRecipeTypeClicked()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    private void EditRecipeView_OnImageDropped()
    {
        throw new System.NotImplementedException();
    }
}