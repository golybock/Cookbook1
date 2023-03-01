using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe.Review;

namespace Models.Models.Database.Client;

public partial class Client
{
    public Client() =>
        ClientImage = new ClientImage{ClientId = Id};
    
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public DateTime DateOfRegistration { get; set; }
    
    public List<ClientImage> ClientImages { get; set; } = new();

    public List<ClientSub> ClientSubOnClients { get; set; } = new();

    public List<ClientSub> ClientSubs { get; set; } = new();

    public List<FavoriteRecipe> FavoriteRecipes { get; set; } = new();

    public List<Recipe.Recipe> Recipes { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();
}