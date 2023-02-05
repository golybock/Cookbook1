using System;

namespace Cookbook.Models.Database.Client;

public partial class FavoriteRecipe
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int ClientId { get; set; }

    public DateTime DateOfAdding { get; set; }

}