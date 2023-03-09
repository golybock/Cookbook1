using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Models.Models.Database;
using Models.Models.Database.Client;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientImageRepository : MainDbClass, IClientImageRepository
{
    public async Task<ClientImage> GetClientImageAsync(int id)
    {
        ClientImage clientImage = new ClientImage();

        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"select * from client_images where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
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
        ClientImage clientImage = new ClientImage();
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"select * from client_images where client_id = $1 order by id desc limit 1 ";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = clientId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
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
        List<ClientImage> clientImages = new List<ClientImage>();
        
        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"select * from client_images where client_id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            
            string query = $"insert into client_images(client_id, image_path)" +
                           $" values ($1, $2) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientImage.ClientId },
                    new() { Value = clientImage.GetImagePath() },
                }
            }; 
            
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                clientImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                result.Value = clientImage;
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

    public async Task<CommandResult> UpdateClientImageAsync(ClientImage clientImage)
    {
        CommandResult result;
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"update client_images set image_path = $2 where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientImage.Id },
                    new() { Value = clientImage.GetImagePath() },
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

    public Task<CommandResult> DeleteClientImageAsync(int id) =>
        DeleteAsync("client_images", id);
}