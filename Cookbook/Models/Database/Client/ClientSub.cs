using System;

namespace Cookbook.Models.Database.Client;

public class ClientSub
{
    public int Id { get; set; }

    public int Sub { get; set; }

    public int ClientId { get; set; }

    public DateTime DateOfSub { get; set; }
}