using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cookbook.Models.Database.Client;

public class ClientImage : INotifyPropertyChanged
{
    private string? _imagePath;
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string? ImagePath
    {
        get => $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients\\" + _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? GetImagePath()
    {
        return _imagePath;
    }

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