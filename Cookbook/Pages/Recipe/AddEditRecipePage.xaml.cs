using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    public RecipeModel Recipe;
    
    public AddEditRecipePage(ClientModel client)
    {
        InitializeComponent();
        Recipe = new RecipeModel();
        DataContext = Recipe;
        MediumPreview.DataContext = Recipe;
    }
    
    public AddEditRecipePage(RecipeModel recipe, ClientModel client)
    {
        InitializeComponent();
        Recipe = recipe;
        DataContext = Recipe;
        MediumPreview.DataContext = Recipe;
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {

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
    

    private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ClearError();
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShowAcceptDialog();
    }

    private void PreviewButton_OnClick(object sender, RoutedEventArgs e)
    {
        // ворк
    }

    private void ClearPage()
    {
        Recipe = new RecipeModel();
        DataContext = Recipe;
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
        Recipe.RecipeImage.ImagePath = path;
        Recipe.RecipeImages.Add(new (){ RecipeId = Recipe.Id, ImagePath = path});
        // отображаем изображение
        if (Recipe.RecipeImage.ImagePath != null)
            RecipeImage.Source = new BitmapImage(new Uri(Recipe.RecipeImage.ImagePath));
    }
}