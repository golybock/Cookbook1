using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;

namespace Cookbook.Database.Repositories;

public class MainDbClass
{
    private string? _connectionString;
    public NpgsqlConnection _connection;
    
    public MainDbClass()
    {
        GetConnectionString();
        
        if (_connectionString != null)
            _connection = new NpgsqlConnection(_connectionString);
        else
            throw new Exception("Error connection");

        TrustConnection();
    }

    private void GetConnectionString()
    {
        var configBuilder = new ConfigurationBuilder().
            AddJsonFile("appsettings.json").Build();
        
        var configSection = configBuilder.GetSection("ConnectionStrings");

        _connectionString = configSection["CookbookDB"] ?? null;
    }

    private bool TrustConnection()
    {
        try
        {
            _connection.Open();
            return true;
        }
        catch
        {
            MessageBox.Show("Не удалось подключиться к базе");
            return false;
        }
        finally
        {
            _connection.Close();
        }
    }

}