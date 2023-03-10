using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cookbook.Models.Database.Client;

public partial class Client : INotifyPropertyChanged
{
    // лайк
    private bool _isLiked;

    // новый путь (если требуется установить путь)
    private string? _newImagePath;
    public ClientImage ClientImage { get; set; }

    public string? NewImagePath
    {
        get => _newImagePath;
        set
        {
            _newImagePath = value;
            OnPropertyChanged();
        }
    }

    public bool IsLiked
    {
        get => _isLiked;
        set
        {
            _isLiked = value;
            OnPropertyChanged();
        }
    }

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