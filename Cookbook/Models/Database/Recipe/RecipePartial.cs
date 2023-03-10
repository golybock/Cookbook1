using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Cookbook.Models.Database.Recipe;

public partial class Recipe : INotifyPropertyChanged
{
    private bool? _isLiked;

    // новый путь (если требуется установить путь)
    private string? _newImagePath;

    private string _text = string.Empty;

    public string? NewImagePath
    {
        get => _newImagePath;
        set
        {
            _newImagePath = value;
            OnPropertyChanged();
        }
    }

    public string? ImagePath =>
        RecipeImage.ImagePath;

    public RecipeImage RecipeImage { get; set; }

    public Category Category { get; set; } = new();

    public bool? IsLiked
    {
        get => _isLiked;
        set
        {
            _isLiked = value;
            OnPropertyChanged();
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            if (value == _text) return;
            _text = value;
            OnPropertyChanged();
        }
    }

    // реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    private string GetText()
    {
        var text = "Нет текста рецепта";

        var path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\" + PathToTextFile + ".txt";

        if (PathToTextFile != null)
            if (File.Exists(path))
                text = File.ReadAllText(path);

        return text;
    }

    public override string ToString()
    {
        var firstLines = $"{Name}\n{Description}\n";

        var secondLines = $"{Category}\n{RecipeType}\n";

        var thirdLines = $"{RecipeStat}\n";

        var fourthLines = $"Шаги приготовления: \n\t{Text}";

        return firstLines + secondLines + thirdLines + fourthLines;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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