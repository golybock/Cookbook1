using System.Windows;
using System.Windows.Controls;

namespace Cookbook.Views.Recipe;

public partial class EditRecipeView : UserControl
{
    public EditRecipeView()
    {
        InitializeComponent();
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

    private void RemoveIngredientFromListButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AddRecipeTypeButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void NewIngredientButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}