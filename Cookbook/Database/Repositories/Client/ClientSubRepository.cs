using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientSubRepository : MainDbClass, IClientSubRepository
{
    public async Task<ClientSub?> GetClientSubAsync(int id)
    {
        var con = GetConnection();
        con.Open();
        try
        {
            ClientSub clientSub = new ClientSub();
            string query = $"select * from client_subs where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientSub.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientSub.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientSub.Sub = reader.GetInt32(reader.GetOrdinal("sub"));
                clientSub.DateOfSub = reader.GetDateTime(reader.GetOrdinal("date_of_sub"));
            }
            
            return clientSub;
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

    public async Task<ClientSub?> GetClientSubAsync(int clientId, int subId)
    {
        var con = GetConnection();
        con.Open();
        try
        {
            ClientSub clientSub = new ClientSub();
            string query = $"select * from client_subs where client_id = $1 and sub = $2";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientId},
                    new () { Value = subId }
                }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientSub.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientSub.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientSub.Sub = reader.GetInt32(reader.GetOrdinal("sub"));
                clientSub.DateOfSub = reader.GetDateTime(reader.GetOrdinal("date_of_sub"));
            }
            
            return clientSub;
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

    public async Task<List<ClientSub>> GetClientSubsAsync(int clientId)
    {
        var con = GetConnection();
        con.Open();
        List<ClientSub> clientSubs = new List<ClientSub>();
        try
        {
            string query = $"select * from client_subs where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = clientId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ClientSub clientSub = new ClientSub();
                clientSub.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientSub.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientSub.Sub = reader.GetInt32(reader.GetOrdinal("sub"));
                clientSub.DateOfSub = reader.GetDateTime(reader.GetOrdinal("date_of_sub"));
                clientSubs.Add(clientSub);
            }
            
            return clientSubs;
        }
        catch
        {
            return new List<ClientSub>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ClientSub>> GetSubsClientAsync(int clientId)
    {
        var con = GetConnection();
        con.Open();
        List<ClientSub> clientSubs = new List<ClientSub>();
        try
        {
            string query = $"select * from client_subs where sub = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = clientId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ClientSub clientSub = new ClientSub();
                clientSub.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientSub.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientSub.Sub = reader.GetInt32(reader.GetOrdinal("sub"));
                clientSub.DateOfSub = reader.GetDateTime(reader.GetOrdinal("date_of_sub"));
                clientSubs.Add(clientSub);
            }
            
            return clientSubs;
        }
        catch
        {
            return new List<ClientSub>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<bool> ClientIsLiked(int clientId, int subId)
    {
        var con = GetConnection();
        con.Open();
        bool result = false;
        try
        {
            string query = $"select * from client_subs where client_id = $1 and sub = $2";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientId },
                    new () { Value = subId }
                }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                result = reader.GetInt32(reader.GetOrdinal("id")) > 0;
            }

            return result;
        }
        catch
        {
            return false;
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddClientSubAsync(ClientSub clientSub)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"insert into client_subs(sub, client_id)" +
                           $" values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientSub.Sub },
                    new() { Value = clientSub.ClientId }
                }
            }; 
            result = CommandResults.Successfully;
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientSub.Id = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateClientSubAsync(ClientSub clientSub)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"update client_subs set sub = $2, client_id = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientSub.Sub },
                    new() { Value = clientSub.ClientId}
                }
            };
            
            result =
                await cmd.ExecuteNonQueryAsync() > 0 ?
                    CommandResults.Successfully :
                    CommandResults.BadRequest; 
            
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

    public async Task<CommandResult> DeleteClientSubAsync(int id)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from client_subs where id = $1";
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

    public async Task<CommandResult> DeleteClientSubAsync(int clientId, int id)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from client_subs where client_id = $1 and sub = $2";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientId },
                    new() { Value = id }
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