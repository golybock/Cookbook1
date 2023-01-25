using System;

namespace Cookbook.Models.Database.Client;

public partial class FavoriteRecipe
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int ClientId { get; set; }

    public DateTime DateOfAdding { get; set; }

    public virtual Database.Client.Client Client { get; set; } = null!;

    public virtual Recipe.Recipe Recipe { get; set; } = null!;
}