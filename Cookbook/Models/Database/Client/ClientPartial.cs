namespace Cookbook.Models.Database.Client;

public partial class Client
{
    public string? ImagePath
    {
        get => ImagePath ?? "../../Resources/not_found_image.png";
        set => ImagePath = value;

    }
    
}