using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientImageRepository : MainDbClass, IClientImageRepository
{
    public async Task<ClientImage?> GetClientImageAsync(int id)
    {
        connection.Open();
        ClientImage clientImage = new ClientImage();
        try
        {
            string query = $"select * from client_images where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                clientImage.DateOfAdded = reader.GetDateTime(reader.GetOrdinal("date_of_added"));
            }

            return clientImage;
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<ClientImage?> GetClientImageByClientIdAsync(int clientId)
    {
        connection.Open();
        ClientImage clientImage = new ClientImage();
        try
        {
            string query = $"select * from client_images where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = clientId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                clientImage.DateOfAdded = reader.GetDateTime(reader.GetOrdinal("date_of_added"));
            }

            return clientImage;
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<ClientImage?>> GetClientImagesAsync(int clientId)
    {
        connection.Open();
        List<ClientImage> clientImages = new List<ClientImage>();
        try
        {
            string query = $"select * from client_images where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = clientId } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ClientImage clientImage = new ClientImage(); 
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                clientImage.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                clientImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                clientImage.DateOfAdded = reader.GetDateTime(reader.GetOrdinal("date_of_added"));
                clientImages.Add(clientImage);
            }

            return clientImages;
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddClientImageAsync(ClientImage clientImage)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into client_images(client_id, image_path)" +
                           $" values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = clientImage.ClientId },
                    new() { Value = clientImage.ImagePath },
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
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update client_images set image_path = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = clientImage.Id },
                    new() { Value = clientImage.ImagePath },
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

    public async Task<CommandResult> DeleteClientImageAsync(int id)
    {
        return await DeleteAsync("client_images", id);
    }
}