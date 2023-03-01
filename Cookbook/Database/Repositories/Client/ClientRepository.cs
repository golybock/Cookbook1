using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Npgsql;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Repositories.Client;

public class ClientRepository : MainDbClass, IClientRepository
{
    public async Task<ClientModel?> GetClientAsync(int id)
    {
        var con = GetConnection();
        con.Open();
        try
        {
            ClientModel client = new ClientModel();
            string query = $"select * from client where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            await con.CloseAsync();
        }
    }

    public async Task<ClientModel?> GetClientAsync(string login)
    {
        var con = GetConnection();
        con.Open();
        try
        {
            ClientModel client = new ClientModel();
            string query = $"select * from client where login = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = login == string.Empty ? DBNull.Value : login  } }
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
            return null;
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        var con = GetConnection();
        con.Open();
        try
        {
            List<ClientModel> clients = new List<ClientModel>();
            string query = $"select * from client";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
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
            return new List<ClientModel>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddClientAsync(ClientModel client)
    {
        var con = GetConnection();
        
        CommandResult result;

        try
        {
            con.Open();
            
            string query = $"insert into client(login, password, name)" +
                           $" values ($1, $2, $3) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = client.Login },
                    new() { Value = client.Password },
                    new() { Value = client.Name == null ? DBNull.Value : client.Name }
                }
            }; 
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            result = CommandResults.Successfully;
            
            while(await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                result.Value = client;
            }

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
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateClientAsync(ClientModel client)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"update client set login = $2, password = $3, name = $4, description = $5 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            
            result =
                await cmd.ExecuteNonQueryAsync() > 0 ?
                    CommandResults.Successfully :
                    CommandResults.NotFulfilled; 
            
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
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteClientAsync(int id)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from client where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = id },
                }
            };
            
            result =
                await cmd.ExecuteNonQueryAsync() > 0 ?
                    CommandResults.Successfully :
                    CommandResults.NotFulfilled; 
            
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
            await con.CloseAsync();
        }
    }
}