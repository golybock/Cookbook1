using System;
using System.Collections.Generic;

namespace Cookbook.Models.Database.Client;

public partial class Client
{
    private string _description;

    private string _name;

    public Client()
    {
        ClientImage = new ClientImage {ClientId = Id};
    }

    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string? Description
    {
        get
        {
            if (string.IsNullOrEmpty(_description))
                return "Описание отсутствует";

            return _description;
        }
        set
        {
            _description = value;

            OnPropertyChanged();
        }
    }

    public DateTime DateOfRegistration { get; set; }

    public List<FavoriteRecipe> FavoriteRecipes { get; set; } = new();

    public List<Recipe.Recipe> Recipes { get; set; } = new();
}