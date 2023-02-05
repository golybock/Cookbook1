using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces;
using Npgsql;

namespace CookbookApi.Database.Repositories.Recipe;

public class RecipeIngredientRepository : MainDbClass, IRecipeIngredientRepository
{
    public async Task<RecipeIngredient> GetRecipeIngredientAsync(int id)
    {
        Connection.Open();
        try
        {
            RecipeIngredient recipeIngredient = new RecipeIngredient();
            string query = $"select * from recipe_ingredients where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));

            }
            
            return recipeIngredient;
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

    public async Task<List<RecipeIngredient>> GetRecipeIngredientByRecipeAsync(int recipeId)
    {
        Connection.Open();
        List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
        try
        {
            string query = $"select * from recipe_ingredients where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters = { new() { Value = recipeId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                RecipeIngredient recipeIngredient = new RecipeIngredient();
                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));
                recipeIngredients.Add(recipeIngredient);
            }
            
            return recipeIngredients;
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

    public async Task<List<RecipeIngredient>> GetRecipeIngredientsAsync()
    {
        Connection.Open();
        List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
        try
        {
            string query = $"select * from recipe_ingredients";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                RecipeIngredient recipeIngredient = new RecipeIngredient();
                recipeIngredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeIngredient.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                recipeIngredient.IngredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id"));
                recipeIngredient.Count = reader.GetInt32(reader.GetOrdinal("count"));
                recipeIngredients.Add(recipeIngredient);
            }
            
            return recipeIngredients;
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

    public async Task<CommandResult> AddRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        CommandResult result;
                Connection.Open();
                try
                {
                    string query = $"insert into recipe_ingredients(recipe_id, ingredient_id, count) values ($1, $2, $3) returning id";
                    await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
                    {
                        Parameters =
                        {
                            new() { Value = recipeIngredient.RecipeId },
                            new() { Value = recipeIngredient.IngredientId },
                            new() { Value = recipeIngredient.Count }
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

    public async Task<CommandResult> UpdateRecipeIngredientAsync(RecipeIngredient recipeIngredient)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"update recipe_ingredients set count = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
            {
                Parameters =
                {
                    new() { Value = recipeIngredient.Id },
                    new() { Value = recipeIngredient.Count }
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

    public async Task<CommandResult> DeleteRecipeIngredientAsync(int id)
    {
        return await DeleteAsync("recipe_ingredients", id);
    }
}