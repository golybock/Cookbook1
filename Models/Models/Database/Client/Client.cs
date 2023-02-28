using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe.Review;

namespace Models.Models.Database.Client;

public partial class Client
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual DateTime DateOfRegistration { get; set; }
    
    public virtual List<ClientImage> ClientImages { get; set; } = new();

    public virtual List<ClientSub> ClientSubOnClients { get; set; } = new();

    public virtual List<ClientSub> ClientSubs { get; set; } = new();

    public virtual List<FavoriteRecipe> FavoriteRecipes { get; set; } = new();

    public virtual List<Recipe.Recipe> Recipes { get; set; } = new();

    public virtual List<Review> Reviews { get; set; } = new();
}