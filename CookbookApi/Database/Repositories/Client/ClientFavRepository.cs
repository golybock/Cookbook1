using Cookbook.Database.Repositories;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using CookbookApi.Database.Repositories.Interfaces.ClientInterfaces;
using Npgsql;

namespace CookbookApi.Database.Repositories.Client;

public class ClientFavRepository : MainDbClass, IClientFavoriteRepository
{
    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        Connection.Open();
        FavoriteRecipe favoriteRecipe = new FavoriteRecipe();
        try
        {
            string query = $"select * from favorite_recipes where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        Connection.Open();
        List<FavoriteRecipe> favoriteRecipes = new List<FavoriteRecipe>();
        try
        {
            string query = $"select * from favorite_recipes where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"insert into favorite_recipes(recipe_id, client_id) values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"update favorite_recipes set recipe_id = $2, client_id = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        return await DeleteAsync("favorite_recipes", id);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"delete from favorite_recipes where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = recipeId },
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

    public async Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"delete from favorite_recipes where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = clientId },
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