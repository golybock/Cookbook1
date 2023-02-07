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
}