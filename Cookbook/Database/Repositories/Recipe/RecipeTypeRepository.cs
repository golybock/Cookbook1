using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Models.Models.Database;
using Models.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class RecipeTypeRepository : MainDbClass, IRecipeTypeRepository
{
    public async Task<RecipeType?> GetRecipeTypeAsync(int id)
    {
        var con = GetConnection();
        RecipeType recipeType = new RecipeType();
        try
        {
            string query = $"select * from recipe_type where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeType.Id = id;
                recipeType.Name = reader.GetString(reader.GetOrdinal("name"));
            }
            
            return recipeType;
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

    public async Task<List<RecipeType>> GetRecipeTypesAsync()
    {
        var con = GetConnection();
        List<RecipeType> recipeTypes = new List<RecipeType>();
        try
        {
            await con.OpenAsync();
            string query = $"select * from recipe_type";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                RecipeType recipeType = new RecipeType();
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
        CommandResult commandResult = CommandResults.Successfully;
        
        var con = GetConnection();
        
        try
        {
            await con.OpenAsync();
            string query = $"insert into recipe_type(name) values($1) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new(){Value = recipeType.Name}}
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                recipeType.Id = reader.GetInt32(reader.GetOrdinal("id"));
            }
            
            return commandResult;
        }
        catch(Exception e)
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