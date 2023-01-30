﻿using System;
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

    public Client.Client Client { get; set; } = null!;

    public List<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();

    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public List<RecipeImage> RecipeImages { get; set; } = new List<RecipeImage>();

    public RecipeStats? RecipeStat { get; set; }

    public RecipeType RecipeType { get; set; } = null!;

    public List<Review.Review> Reviews { get; set; } = new List<Review.Review>();
}