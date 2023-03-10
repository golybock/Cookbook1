using ModernWpf.Controls;

namespace Cookbook.ContentDialogs;

// start refactor to enum
public class ContentDialogs
{

    public static async void ShowErrorDialog(string error)
    {
        var errorDialog = new ContentDialog
        {
            Title = "Ошибка",
            Content = error,
            CloseButtonText = "Закрыть"
        };

        await errorDialog.ShowAsync();
    }
    
    
    
}