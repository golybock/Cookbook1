using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces;
using Npgsql;

namespace CookbookApi.Database.Repositories.Recipe;

public class RecipeImageRepository : MainDbClass, IRecipeImageRepository
{
    public async Task<RecipeImage> GetRecipeImageAsync(int id)
    {
        Connection.Open();
        try
        {
            RecipeImage recipeImage = new RecipeImage();
            string query = $"select * from recipe_images where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeImage.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                recipeImage.ImageNumber = reader.GetInt32(reader.GetOrdinal("image_number"));
            }
            
            return recipeImage;
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

    public async Task<RecipeImage> GetRecipeImageByRecipeAsync(int recipeId)
    {
        Connection.Open();
        try
        {
            RecipeImage recipeImage = new RecipeImage();
            string query = $"select * from recipe_images where recipe_id = $1 limit 1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = recipeId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeImage.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeImage.ImagePath = reader.GetString(reader.GetOrdinal("image_path"));
                recipeImage.ImageNumber = reader.GetInt32(reader.GetOrdinal("image_number"));
            }
            
            return recipeImage;
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

    public async Task<List<RecipeImage>> GetRecipeImagesAsync(int recipeId)
    {
        Connection.Open();
        List<RecipeImage> recipeImages = new List<RecipeImage>();
        try
        {
            string query = $"select * from recipe_images where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
                recipeImage.ImageNumber = reader.GetInt32(reader.GetOrdinal("image_number"));
                recipeImages.Add(recipeImage);
            }
            
            return recipeImages;
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

    public async Task<CommandResult> AddRecipeImageAsync(RecipeImage recipeImage)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"insert into recipe_images(recipe_id, image_path, image_number) values ($1, $2, $3) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = recipeImage.RecipeId },
                    new() { Value = recipeImage.ImagePath },
                    new() { Value = recipeImage.ImageNumber }
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

    public async Task<CommandResult> UpdateRecipeImageAsync(RecipeImage recipeImage)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"update recipe_images set image_path = $2, image_number = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = recipeImage.Id },
                    new() { Value = recipeImage.ImagePath },
                    new() { Value = recipeImage.ImageNumber }
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

    public async Task<CommandResult> DeleteRecipeImageAsync(int id)
    {
        return await DeleteAsync("recipe_images", id);
    }
}