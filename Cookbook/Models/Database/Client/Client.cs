using System;
using System.Collections.Generic;
using Cookbook.Models.Database.Recipe.Review;

namespace Cookbook.Models.Database.Client;

public partial class Client
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime DateOfRegistration { get; set; }
    
    public virtual List<ClientImage?> ClientImages { get; set; } = new List<ClientImage>();

    public virtual List<ClientSub> ClientSubClients { get; set; } = new List<ClientSub>();

    public virtual List<ClientSub> ClientSubSubNavigations { get; set; } = new List<ClientSub>();

    public virtual List<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();

    public virtual List<Recipe.Recipe> Recipes { get; set; } = new List<Recipe.Recipe>();

    public virtual List<Review> Reviews { get; set; } = new List<Review>();
}