namespace Models.Models.Database.Recipe;

public partial class Recipe
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ? "../../Resources/sirniki.png" : _imagePath;
        set => _imagePath = value;
    }
    
    public string? Category { get; set; }
    public bool? IsLiked { get; set; } = false;
    public decimal? Rating { get; set; }
    public event EventHandler Delete;
    public event EventHandler Edit;
    public event EventHandler Like;
}