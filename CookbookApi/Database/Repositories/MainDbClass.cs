using Cookbook.Models.Database;
using Npgsql;

namespace CookbookApi.Database.Repositories;

public class MainDbClass
{
    private string? _connectionString;
    public NpgsqlConnection Connection;
    
    public MainDbClass()
    {
        GetConnectionString();
        
        if (_connectionString != null)
            Connection = new NpgsqlConnection(_connectionString);
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
            Connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            Connection.Close();
        }
    }
    
    public async Task<CommandResult> DeleteAsync(string table, int id)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"delete from {table} where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

}