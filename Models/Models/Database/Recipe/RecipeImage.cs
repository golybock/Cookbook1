using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Models.Database.Recipe;

public class RecipeImage : INotifyPropertyChanged
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ?
            "../../Resources/not_found_image.png" :
            $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\" + _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();   
        }
    }
    
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