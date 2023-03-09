using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class MeasureRepository : MainDbClass, IMeasureRepository
{
    public async Task<Measure> GetMeasureAsync(int id)
    {
        Measure measure = new Measure();
        
        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"select * from measure where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                measure.Id = reader.GetInt32(reader.GetOrdinal("id"));
                measure.Name = reader.GetString(reader.GetOrdinal("name"));
            }
            
            return measure;
        }
        catch
        {
            return new Measure();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<Measure>> GetMeasuresAsync()
    {
        List<Measure> measures = new List<Measure>();
        
        var con = GetConnection();
     
        try
        {
            con.Open();
            
            string query = $"select * from measure";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                Measure measure = new Measure();
                measure.Id = reader.GetInt32(reader.GetOrdinal("id"));
                measure.Name = reader.GetString(reader.GetOrdinal("name"));
                measures.Add(measure);
            }
            
            return measures;
        }
        catch
        {
            return new List<Measure>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddMeasureAsync(Measure measure)
    {
        CommandResult result;
        
        var con = GetConnection();

        try
        {
            con.Open();
            
            string query = $"insert into measure(name) values ($1) returning id";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = measure.Id },
                    new() { Value = measure.Name }
                }
            };
            
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                measure.Id = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateMeasureAsync(Measure measure)
    {
        CommandResult result;

        var con = GetConnection();
        
        try
        {
            await con.OpenAsync();
            
            string query = $"update measure set name = $2 where id = $1";
            
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = measure.Id },
                    new() { Value = measure.Name }
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

    public async Task<CommandResult> DeleteMeasureAsync(int id) =>
        await DeleteAsync("measure", id);
}