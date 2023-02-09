namespace Models.Models.Database.Client;

public partial class Client
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ? "../../Resources/sirniki.png" : _imagePath;
        set => _imagePath = value;
    }
    public int PostCount { get; set; }
    public event EventHandler Delete;
    public event EventHandler Edit;
}