using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Models.Database.Recipe;

public partial class Recipe : INotifyPropertyChanged
{
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
    
    public string? ImagePath =>
        RecipeImage.ImagePath;

    public RecipeImage RecipeImage { get; set; }

    public Category Category { get => _category;
        set
        {
            _category = value;
        } 
    }

    private Category _category = new ();

    private bool? _isLiked;
    
    private string _text = string.Empty;

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

    private string GetText()
    {
        string text = "Нет текста рецепта";

        string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\" + PathToTextFile + ".txt";
        
        if (PathToTextFile != null)
            if (File.Exists(path))
                text = File.ReadAllText(path);

        return text;
    }

    public override string ToString()
    {
        string firstLines = $"{Name}\n{Description}\n";

        string secondLines = $"{Category}\n{RecipeType}\n";

        string thirdLines = $"{RecipeStat}";

        string fourthLines = $"{Text}";
        
        return firstLines + secondLines + thirdLines + fourthLines;
    }

    // реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

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