using System;

namespace Cookbook.Models.Database.Client;

public partial class ClientImage
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string? ImagePath { get; set; } = null!;

    public DateTime DateOfAdded { get; set; }
}