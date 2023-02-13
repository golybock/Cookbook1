using System.ComponentModel;

namespace Models.Models.Database.Client;

public partial class Client
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ? "../../Resources/sirniki.png" : _imagePath;
        set => _imagePath = value;
    }
    public int PostCount => Recipes.Count;
    private bool? _isLiked;
    public bool? IsLiked
    {
        get => _isLiked;
        set
        {
            _isLiked = value;
            OnPropertyChanged(new PropertyChangedEventArgs("IsLiked"));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, e);
        }
    }

}