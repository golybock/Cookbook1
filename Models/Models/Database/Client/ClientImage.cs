using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Models.Database.Client;

public partial class ClientImage : INotifyPropertyChanged
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    private string? _imagePath;

    public string? ImagePath
    {
        get => $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients\\" + _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public string? GetImagePath()
    {
        return _imagePath;
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