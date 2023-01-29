﻿using System;

namespace Cookbook.Models.Database.Recipe;

public partial class Recipe
{
    private string? _imagePath;
    public string? ImagePath
    {
        get => String.IsNullOrEmpty(_imagePath) ? "../../Resources/not_found_image.png" : _imagePath;
        set => _imagePath = value;
    }
}