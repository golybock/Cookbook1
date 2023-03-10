using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class IngredientRepository : RepositoryBase, IIngredientRepository
{
    public async Task<Ingredient> GetIngredientAsync(int id)
    {
        var ingredient = new Ingredient();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from ingredient where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                ingredient.MeasureId = reader.GetInt32(reader.GetOrdinal("measure_id"));
                ingredient.Name = reader.GetString(reader.GetOrdinal("name"));
            }

            return ingredient;
        }
        catch
        {
            return new Ingredient();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        var ingredients = new List<Ingredient>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from ingredient";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var ingredient = new Ingredient();
                ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));
                ingredient.MeasureId = reader.GetInt32(reader.GetOrdinal("measure_id"));
                ingredient.Name = reader.GetString(reader.GetOrdinal("name"));

                ingredients.Add(ingredient);
            }

            return ingredients;
        }
        catch
        {
            return new List<Ingredient>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddIngredientAsync(Ingredient ingredient)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into ingredient(measure_id, name)" +
                        " values ($1, $2) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = ingredient.Measure.Id},
                    new NpgsqlParameter {Value = ingredient.Name}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) ingredient.Id = reader.GetInt32(reader.GetOrdinal("id"));

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

    public async Task<CommandResult> UpdateIngredientAsync(Ingredient ingredient)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            await con.OpenAsync();

            var query = "update ingredient set measure_id = $2, name = $3 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = ingredient.Id},
                    new NpgsqlParameter {Value = ingredient.MeasureId},
                    new NpgsqlParameter {Value = ingredient.Name}
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

    public async Task<CommandResult> DeleteIngredientAsync(int id)
    {
        return await DeleteAsync("ingredient", id);
    }
}