using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Models.Models.Database;
using Models.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeImageRepository : MainDbClass, IRecipeImageRepository
{
    public async Task<RecipeImage> GetRecipeImageAsync(int id)
    {
        RecipeImage recipeImage = new RecipeImage();
        
        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"select * from recipe_images where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeImage.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
            }
            
            return recipeImage;
        }
        catch
        {
            return new RecipeImage();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<RecipeImage> GetRecipeImageByRecipeAsync(int recipeId)
    {
        RecipeImage recipeImage = new RecipeImage();
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"select * from recipe_images where recipe_id = $1 limit 1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = recipeId} }
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeImage.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
            }
            
            return recipeImage;
        }
        catch
        {
            return new RecipeImage();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeImage>> GetRecipeImagesAsync(int recipeId)
    {
        List<RecipeImage> recipeImages = new List<RecipeImage>();
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"select * from recipe_images where recipe_id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = recipeId} }
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                RecipeImage recipeImage = new RecipeImage();
                
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeImage.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                
                recipeImages.Add(recipeImage);
            }
            
            return recipeImages;
        }
        catch
        {
            return new List<RecipeImage>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage)
    {
        CommandResult result;
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"insert into recipe_images(recipe_id, image_path)" +
                           $" values ($1, $2) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipeImage.RecipeId },
                    new() { Value = recipeImage.GetImagePath() }
                }
            }; 
            
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage)
    {
        CommandResult result;
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"update recipe_images set image_path = $2 where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipeImage.Id },
                    new() { Value = recipeImage.GetImagePath() }
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

    public async Task<CommandResult> DeleteRecipeImagesByRecipeAsync(int id)
    {
        CommandResult result;
        
        var con = GetConnection();
        
        try
        {
            con.Open();
            
            string query = $"delete from recipe_images where recipe_id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = id },
                }
            };
            
            result = await cmd.ExecuteNonQueryAsync() > 0 ?
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
    
    public async Task<CommandResult> DeleteRecipeImageAsync(int id) =>
        await DeleteAsync("recipe_images", id);
}