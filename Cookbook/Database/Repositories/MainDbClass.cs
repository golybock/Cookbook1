using System;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Models.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Models.Models.Database;
using Npgsql;

namespace Cookbook.Database.Repositories;

public class MainDbClass
{
    private string? _connectionString;

    public MainDbClass()
    {
        GetConnectionString();

        TrustConnection();
    }

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
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
        var connection = new NpgsqlConnection(_connectionString);
        
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
        
        var connection = new NpgsqlConnection(_connectionString);
        
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