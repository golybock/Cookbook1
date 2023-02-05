using Cookbook.Database.Repositories;
using Cookbook.Models.Database;
using CookbookApi.Database.Repositories.Interfaces.ClientInterfaces;
using Npgsql;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace CookbookApi.Database.Repositories.Client;

public class ClientRepository : MainDbClass, IClientRepository
{
    public async Task<ClientModel> GetClientAsync(int id)
    {
        Connection.Open();
        try
        {
            ClientModel client = new ClientModel();
            string query = $"select * from client where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();
                client.DateOfRegistration = reader.GetDateTime(reader.GetOrdinal("date_of_registration"));
            }
            
            return client;
        }
        catch
        {
            return null;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    public async Task<ClientModel> GetClientAsync(string login)
    {
        Connection.Open();
        try
        {
            ClientModel client = new ClientModel();
            string query = $"select * from client where login = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = login} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();
                client.DateOfRegistration = reader.GetDateTime(reader.GetOrdinal("date_of_registration"));
            }
            
            return client;
        }
        catch(Exception e)
        {
            return new ClientModel(){Id = 5};
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        Connection.Open();
        try
        {
            List<ClientModel> clients = new List<ClientModel>();
            string query = $"select * from client";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ClientModel client = new ClientModel();
                
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();
                client.DateOfRegistration = reader.GetDateTime(reader.GetOrdinal("date_of_registration"));
                
                clients.Add(client);
            }
            
            return clients;
        }
        catch
        {
            return null;
        }
        finally
        {
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddClientAsync(ClientModel client)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"insert into client(login, password, name, description)" +
                           $" values ($1, $2, $3, $4) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = client.Login },
                    new() { Value = client.Password },
                    new() { Value = client.Name },
                    new() { Value = client.Description }
                }
            }; 
            result = CommandResults.Successfully;
            result.ValueId = await cmd.ExecuteNonQueryAsync();
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

    public async Task<CommandResult> UpdateClientAsync(ClientModel client)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"update client set login = $2, password = $3, name = $4, description = $5 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = client.Id },
                    new() { Value = client.Login },
                    new() { Value = client.Password },
                    new() { Value = client.Name },
                    new() { Value = client.Description }
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

    public async Task<CommandResult> DeleteClientAsync(int id)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"delete from client where id = $1";
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