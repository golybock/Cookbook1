using System;
using System.Collections.Generic;

namespace Cookbook.Models.Database.Recipe;

public partial class Recipe
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int RecipeTypeId { get; set; }
    
    public string Name { get; set; }

    public DateTime DateOfCreation { get; set; }

    public string? Description { get; set; }

    public string? PathToTextFile { get; set; }

    public int PortionCount { get; set; }

    public int CookingTime { get; set; }

    public virtual Client.Client Client { get; set; } = null!;

    public virtual List<RecipeCategory> RecipeCategories { get; } = new List<RecipeCategory>();

    public virtual List<RecipeIngredient> RecipeIngredients { get; } = new List<RecipeIngredient>();

    public virtual RecipeStats? RecipeStat { get; set; }

    public virtual RecipeType RecipeType { get; set; } = null!;

    public virtual List<Review.Review> Reviews { get; } = new List<Review.Review>();
}