using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cookbook.Models.Database.Client;

namespace Models.Models.Database.Client;

public partial class Client: INotifyPropertyChanged
{
    private string? _newImagePath;
    
    // новый путь (если требуется установить путь)
    public string? NewImagePath
    {
        get => _newImagePath;
        set
        {
            _newImagePath = value;
            ClientImage.ImagePath = _newImagePath;
            OnPropertyChanged();
        }
    }
    
    private bool? _isLiked;
    
    public bool? IsLiked
    {
        get => _isLiked;
        set
        {
            _isLiked = value;
            OnPropertyChanged();
        }
    }
    
    public ClientImage ClientImage { get; set; }
    
    public int PostCount => Recipes.Count;
    


    public Client()
    {
        ClientImage = new ClientImage{ClientId = Id};
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