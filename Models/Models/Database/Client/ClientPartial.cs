using System.ComponentModel;
using Cookbook.Models.Database.Client;

namespace Models.Models.Database.Client;

public partial class Client: INotifyPropertyChanged
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ?
            "../../Resources/not_found_image.png" :
            $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients\\" + _imagePath;
        set => _imagePath = value;
    }

    public ClientImage ClientImage { get; set; } = new ClientImage();
    
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