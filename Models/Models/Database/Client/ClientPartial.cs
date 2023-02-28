using System.ComponentModel;
using Cookbook.Models.Database.Client;

namespace Models.Models.Database.Client;

public partial class Client: INotifyPropertyChanged
{
    private string? _newImagePath;
    
    // новый путь (если требуется установить путь)
    public string? NewImagePath
    {
        get => _newImagePath;
        set
        {
            _newImagePath = value;
            ClientImage.ImagePath = _newImagePath;
            OnPropertyChanged(new PropertyChangedEventArgs("Client.ClientImage.ImagePath"));
        }
    }
    
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
    
    public ClientImage ClientImage { get; set; }
    
    public int PostCount => Recipes.Count;
    


    public Client()
    {
        ClientImage = new ClientImage{ClientId = Id};
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