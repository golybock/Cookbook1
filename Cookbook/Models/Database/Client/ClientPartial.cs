using System;

namespace Cookbook.Models.Database.Client;

public partial class Client
{
    private string? _imagePath = null;
    public string? ImagePath
    {
        get => _imagePath ?? "../../Resources/not_found_image.png";
        set => _imagePath = value;

    }

}