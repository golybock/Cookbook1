using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cookbook.Models.Database.Recipe;

public class RecipeImage : INotifyPropertyChanged
{
    private string? _imagePath;
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string? ImagePath
    {
        get
        {
            if (_imagePath != null)
                return $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\" + _imagePath;

            return "../../Resources/not_found_image.png";
        }
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    // реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    // только имя файл
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