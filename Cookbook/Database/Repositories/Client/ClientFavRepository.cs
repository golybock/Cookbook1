using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientFavRepository : MainDbClass, IClientFavoriteRepository
{
    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        connection.Open();
        FavoriteRecipe favoriteRecipe = new FavoriteRecipe();
        try
        {
            string query = $"select * from favorite_recipes where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                
                favoriteRecipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                favoriteRecipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                favoriteRecipe.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                favoriteRecipe.DateOfAdding = reader.GetDateTime(reader.GetOrdinal("date_of_adding"));
            }

            return favoriteRecipe;
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

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        connection.Open();
        List<FavoriteRecipe> favoriteRecipes = new List<FavoriteRecipe>();
        try
        {
            string query = $"select * from favorite_recipes where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = clientId } }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                FavoriteRecipe favoriteRecipe = new FavoriteRecipe();
                favoriteRecipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                favoriteRecipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                favoriteRecipe.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                favoriteRecipe.DateOfAdding = reader.GetDateTime(reader.GetOrdinal("date_of_adding"));
                favoriteRecipes.Add(favoriteRecipe);
            }

            return favoriteRecipes;
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

    public async Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into favorite_recipes(recipe_id, client_id) values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = favoriteRecipe.RecipeId },
                    new() { Value = favoriteRecipe.ClientId },
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

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update favorite_recipes set recipe_id = $2, client_id = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = favoriteRecipe.Id },
                    new() { Value = favoriteRecipe.RecipeId },
                    new() { Value = favoriteRecipe.RecipeId }
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

    public async Task<CommandResult> DeleteFavoriteClientAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from favorite_recipes where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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
            await connection.CloseAsync();
        }
    }
}