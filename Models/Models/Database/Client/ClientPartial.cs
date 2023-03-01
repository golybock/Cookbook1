using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Models.Database.Client;

public partial class Client: INotifyPropertyChanged
{
    public int PostCount =>
        Recipes.Count;

    public string? ImagePath =>
        ClientImage.ImagePath;
    
    public ClientImage ClientImage { get; set; }
    
    // новый путь (если требуется установить путь)
    private string? _newImagePath;

    public string? NewImagePath
    {
        get => _newImagePath;
        set
        {
            _newImagePath = value;
            OnPropertyChanged();
        }
    }
    
    // лайк
    private bool _isLiked;
    
    public bool IsLiked
    {
        get => _isLiked;
        set
        {
            _isLiked = value;
            OnPropertyChanged();
        }
    }
    
    // для привзяки like
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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