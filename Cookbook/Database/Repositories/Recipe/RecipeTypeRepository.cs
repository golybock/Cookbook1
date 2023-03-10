using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeTypeRepository : RepositoryBase, IRecipeTypeRepository
{
    public async Task<RecipeType> GetRecipeTypeAsync(int id)
    {
        var con = GetConnection();
        var recipeType = new RecipeType();
        try
        {
            await con.OpenAsync();
            var query = "select * from recipe_type where id = $1";
            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                recipeType.Id = id;
                recipeType.Name = reader.GetString(reader.GetOrdinal("name"));
            }

            return recipeType;
        }
        catch
        {
            return new();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<RecipeType>> GetRecipeTypesAsync()
    {
        var con = GetConnection();
        var recipeTypes = new List<RecipeType>();
        try
        {
            await con.OpenAsync();
            var query = "select * from recipe_type";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var recipeType = new RecipeType();
                recipeType.Id = reader.GetInt32(reader.GetOrdinal("id"));
                recipeType.Name = reader.GetString(reader.GetOrdinal("name"));
                recipeTypes.Add(recipeType);
            }

            return recipeTypes;
        }
        catch
        {
            return new List<RecipeType>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddRecipeTypeAsync(RecipeType recipeType)
    {
        var commandResult = CommandResults.Successfully;

        var con = GetConnection();

        try
        {
            await con.OpenAsync();
            var query = "insert into recipe_type(name) values($1) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = recipeType.Name}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) recipeType.Id = reader.GetInt32(reader.GetOrdinal("id"));

            return commandResult;
        }
        catch (Exception e)
        {
            commandResult = CommandResults.BadRequest;
            commandResult.Description = e.ToString();
            return commandResult;
        }
        finally
        {
            await con.CloseAsync();
        }
    }
}