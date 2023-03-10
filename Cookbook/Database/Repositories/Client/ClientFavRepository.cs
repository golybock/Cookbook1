using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Npgsql;

namespace Cookbook.Database.Repositories.Client;

public class ClientFavRepository : RepositoryBase, IClientFavoriteRepository
{
    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        var favoriteRecipe = new FavoriteRecipe();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from favorite_recipes where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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
            return new FavoriteRecipe();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        var favoriteRecipes = new List<FavoriteRecipe>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from favorite_recipes where client_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = clientId}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var favoriteRecipe = new FavoriteRecipe();

                favoriteRecipe.Id = reader.GetInt32(reader.GetOrdinal("id"));
                favoriteRecipe.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                favoriteRecipe.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));

                favoriteRecipes.Add(favoriteRecipe);
            }

            return favoriteRecipes;
        }
        catch
        {
            return new List<FavoriteRecipe>();
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
        var result = false;
        try
        {
            var query = "select * from favorite_recipes where client_id = $1 and recipe_id = $2";
            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = clientId},
                    new NpgsqlParameter {Value = recipeId}
                }
            };
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) result = reader.GetInt32(reader.GetOrdinal("id")) > 0;

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
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into favorite_recipes(recipe_id, client_id)" +
                        " values ($1, $2) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = favoriteRecipe.RecipeId},
                    new NpgsqlParameter {Value = favoriteRecipe.ClientId}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) favoriteRecipe.Id = reader.GetInt32(reader.GetOrdinal("id"));

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

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update favorite_recipes set recipe_id = $2, client_id = $3 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = favoriteRecipe.Id},
                    new NpgsqlParameter {Value = favoriteRecipe.RecipeId},
                    new NpgsqlParameter {Value = favoriteRecipe.RecipeId}
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

    public Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        return DeleteAsync("favorite_recipes", id);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int recipeId, int clientId)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "delete from favorite_recipes where recipe_id = $1 and client_id = $2";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeId},
                    new NpgsqlParameter {Value = clientId}
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

    public async Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "delete from favorite_recipes where recipe_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = recipeId}
                }
            };

            result =
                await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.BadRequest;

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

    public async Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "delete from favorite_recipes where client_id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = clientId}
                }
            };

            result =
                await cmd.ExecuteNonQueryAsync() > 0 ? CommandResults.Successfully : CommandResults.BadRequest;

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