using Cookbook.Models.Database.Client;

namespace Models.Models.Database.Client;

public partial class Client
{
    public Client() =>
        ClientImage = new ClientImage{ClientId = Id};
    
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    private string _name;

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _description;

    public string? Description
    {
        get => _description;
        set
        {
            _description = _name;
            OnPropertyChanged();
        }
    }

    public DateTime DateOfRegistration { get; set; }
    
    public List<FavoriteRecipe> FavoriteRecipes { get; set; } = new();

    public List<Recipe.Recipe> Recipes { get; set; } = new();
}