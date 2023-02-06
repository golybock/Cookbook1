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
    
    public List<ClientImage?> ClientImages { get; set; } = new List<ClientImage>();

    public List<ClientSub> ClientSubOnClients { get; set; } = new List<ClientSub>();

    public List<ClientSub> ClientSubs { get; set; } = new List<ClientSub>();

    public List<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();

    public List<global::Models.Models.Database.Recipe.Recipe> Recipes { get; set; } = new List<global::Models.Models.Database.Recipe.Recipe>();

    public List<Review> Reviews { get; set; } = new List<Review>();
}