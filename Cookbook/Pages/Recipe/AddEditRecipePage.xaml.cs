using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Cookbook.Database.Services;
using Microsoft.Win32;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    private RecipeModel _recipe;
    private ClientModel _client;
    private RecipeService _recipeService;
    
    public AddEditRecipePage(ClientModel client)
    {
        _client = client;
        
        _recipe = new RecipeModel();
        _recipeService = new RecipeService(_client);

        InitializeComponent();
        
        DataContext = _recipe;
        MediumPreview.DataContext = _recipe;
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        _recipe = recipe;
        _client = client;
        
        _recipe = new RecipeModel();
        _recipeService = new RecipeService(_client);
        
        InitializeComponent();
        
        DataContext = _recipe;
        MediumPreview.DataContext = _recipe;
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        AddRecipe();
    }

    private async Task AddRecipe()
    {
        await _recipeService.AddRecipeAsync(_recipe);
    }

    private void OutError(string error)
    {
        ErrorTextBlock.Text = error;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }

    private void ClearError()
    {
        ErrorTextBlock.Text = null;
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
    

    private void Input(object sender, TextChangedEventArgs e)
    {
        ClearError();
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShowAcceptDialog();
    }

    private void ClearPage()
    {
        _recipe = new RecipeModel();
        
        DataContext = null;
        
        DataContext = _recipe;
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

    private void ImageView_OnDrop(object sender, DragEventArgs e)
    {
        string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        string file = files[0];
        // если файл картинка
        if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            SetImage(file);
    }
    
    private void SetImage(string path)
    {
        // сохраняем путь в объекте
        _recipe.RecipeImage.ImagePath = path;
        _recipe.RecipeImages.Add(new (){ RecipeId = _recipe.Id, ImagePath = path});
        // отображаем изображение
        if (_recipe.RecipeImage.ImagePath != null)
            RecipeImage.Source = new BitmapImage(new Uri(_recipe.RecipeImage.ImagePath));
    }

    private void ImageView_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ChooseImage();
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
    
}