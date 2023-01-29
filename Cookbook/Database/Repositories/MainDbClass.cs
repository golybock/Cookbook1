using System;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Models.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;

namespace Cookbook.Database.Repositories;

public class MainDbClass
{
    private string? _connectionString;
    public NpgsqlConnection connection;
    
    public MainDbClass()
    {
        GetConnectionString();
        
        if (_connectionString != null)
            connection = new NpgsqlConnection(_connectionString);
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
            connection.Open();
            return true;
        }
        catch
        {
            MessageBox.Show("Не удалось подключиться к базе");
            return false;
        }
        finally
        {
            connection.Close();
        }
    }
    
    public async Task<CommandResult> DeleteAsync(string table, int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from {table} where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = id },
                }
            };
            result = await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.BadRequest; 
            return result;
        }
        catch(Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
            return result;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

}