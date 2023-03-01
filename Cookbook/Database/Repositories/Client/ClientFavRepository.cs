using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientFavRepository : MainDbClass, IClientFavoriteRepository
{
    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        var con = GetConnection();
        con.Open();
        FavoriteRecipe favoriteRecipe = new FavoriteRecipe();
        try
        {
            string query = $"select * from favorite_recipes where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            await con.CloseAsync();
        }
    }

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        var con = GetConnection();
        con.Open();
        List<FavoriteRecipe> favoriteRecipes = new List<FavoriteRecipe>();
        try
        {
            string query = $"select * from favorite_recipes where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
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
            await con.CloseAsync();
        }
    }

    public async Task<bool> GetRecipeIsLiked(int recipeId, int clientId)
    {
        var con = GetConnection();
        con.Open();
        bool result = false;
        try
        {
            string query = $"select * from favorite_recipes where client_id = $1 and recipe_id = $2";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientId },
                    new () { Value = recipeId }
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

    public async Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"insert into favorite_recipes(recipe_id, client_id)" +
                           $" values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = favoriteRecipe.RecipeId },
                    new() { Value = favoriteRecipe.ClientId },
                }
            }; 
            
            result = CommandResults.Successfully;
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                favoriteRecipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"update favorite_recipes set recipe_id = $2, client_id = $3 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = favoriteRecipe.Id },
                    new() { Value = favoriteRecipe.RecipeId },
                    new() { Value = favoriteRecipe.RecipeId }
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

    public Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        return DeleteAsync("favorite_recipes", id);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int recipeId, int clientId)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from favorite_recipes where recipe_id = $1 and client_id = $2";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipeId },
                    new () { Value = clientId }
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

    public async Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from favorite_recipes where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = recipeId },
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

    public async Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        var con = GetConnection();
        CommandResult result;
        con.Open();
        try
        {
            string query = $"delete from favorite_recipes where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = clientId },
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
}