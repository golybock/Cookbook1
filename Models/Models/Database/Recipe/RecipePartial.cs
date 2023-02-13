using System.ComponentModel;

namespace Models.Models.Database.Recipe;

public partial class Recipe : INotifyPropertyChanged
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ? "../../Resources/sirniki.png" : _imagePath;
        set => _imagePath = value;
    }
    
    public string? Category { get; set; }
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
    public decimal? Rating { get; set; }

    public TimeOnly Time => TimeOnly.Parse(new DateTime(0,0,0,0,CookingTime,0).ToString());

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, e);
        }
    }
}