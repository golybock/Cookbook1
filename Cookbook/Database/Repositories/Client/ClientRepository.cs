using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Npgsql;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Repositories.Client;

public class ClientRepository : RepositoryBase, IClientRepository
{
    public async Task<ClientModel> GetClientAsync(int id)
    {
        var client = new ClientModel();

        var con = GetConnection();
        try
        {
            con.Open();

            var query = "select * from client where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();
            }

            return client;
        }
        catch
        {
            return new ClientModel();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<ClientModel> GetClientAsync(string login)
    {
        var client = new ClientModel();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from client where login = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = login == string.Empty ? DBNull.Value : login}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();
            }

            return client;
        }
        catch (Exception e)
        {
            return new ClientModel();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ClientModel>> GetClientsAsync()
    {
        var clients = new List<ClientModel>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from client";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var client = new ClientModel();

                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                client.Login = reader.GetString(reader.GetOrdinal("login"));
                client.Password = reader.GetString(reader.GetOrdinal("password"));
                var name = reader.GetValue(reader.GetOrdinal("name"));
                client.Name = name == DBNull.Value ? null : name.ToString();
                var description = reader.GetValue(reader.GetOrdinal("description"));
                client.Description = description == DBNull.Value ? null : description.ToString();

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
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into client(login, password, name)" +
                        " values ($1, $2, $3) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = client.Login},
                    new NpgsqlParameter {Value = client.Password},
                    new NpgsqlParameter {Value = client.Name == null ? DBNull.Value : client.Name}
                }
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            result = CommandResults.Successfully;

            while (await reader.ReadAsync())
            {
                client.Id = reader.GetInt32(reader.GetOrdinal("id"));
                result.Value = client;
            }

            return result;
        }
        catch (Exception e)
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
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update client set name = $2, description = $3 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = client.Id},
                    new NpgsqlParameter {Value = client.Name == null ? DBNull.Value : client.Name},
                    new NpgsqlParameter {Value = client.Description == null ? DBNull.Value : client.Description}
                }
            };

            result =
                await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.NotFulfilled;

            return result;
        }
        catch (Exception e)
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
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "delete from client where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = id}
                }
            };

            result =
                await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.NotFulfilled;

            return result;
        }
        catch (Exception e)
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