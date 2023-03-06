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

    public Category Category { get; set; } = new();
    // Recipe.Category.Name
    
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
    
    public string? Text { get; set; }

    private string GetText()
    {
        string text = "Нет рецепта";

        string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\" + PathToTextFile;
        
        if (PathToTextFile != null)
            if (File.Exists(PathToTextFile))
                text = File.ReadAllText(PathToTextFile);

        return text;
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