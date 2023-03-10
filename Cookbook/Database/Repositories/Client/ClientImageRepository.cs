using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientImageRepository : RepositoryBase, IClientImageRepository
{
    public async Task<ClientImage> GetClientImageAsync(int id)
    {
        var clientImage = new ClientImage();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from client_images where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
            }

            return clientImage;
        }
        catch
        {
            return new ClientImage();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<ClientImage> GetClientImageByClientIdAsync(int clientId)
    {
        var clientImage = new ClientImage();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from client_images where client_id = $1 order by id desc limit 1 ";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = clientId}}
            };
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
            }

            return clientImage;
        }
        catch
        {
            return new ClientImage();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ClientImage>> GetClientImagesAsync(int clientId)
    {
        var clientImages = new List<ClientImage>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from client_images where client_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = clientId}}
            };
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var clientImage = new ClientImage();
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                clientImages.Add(clientImage);
            }

            return clientImages;
        }
        catch
        {
            return new List<ClientImage>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddClientImageAsync(ClientImage clientImage)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into client_images(client_id, image_path)" +
                        " values ($1, $2) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = clientImage.ClientId},
                    new NpgsqlParameter {Value = clientImage.GetImagePath()}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                result.Value = clientImage;
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

    public async Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update client_images set image_path = $2 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = clientImage.Id},
                    new NpgsqlParameter {Value = clientImage.GetImagePath()}
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

    public Task<CommandResult> DeleteClientImageAsync(int id)
    {
        return DeleteAsync("client_images", id);
    }
}