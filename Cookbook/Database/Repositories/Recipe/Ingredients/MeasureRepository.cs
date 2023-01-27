﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class MeasureRepository : MainDbClass, IMeasureRepository
{
    public async Task<Measure> GetMeasureAsync(int id)
    {
        connection.Open();
        try
        {
            Measure measure = new Measure();
            string query = $"select * from measure where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Measure>> GetMeasuresAsync()
    {
        connection.Open();
        try
        {
            List<Measure> measures = new List<Measure>();
            string query = $"select * from measure";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
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
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddMeasureAsync(Measure measure)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into measure(name) values ($1) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = measure.Id },
                    new() { Value = measure.Name }
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

    public async Task<CommandResult> UpdateMeasureAsync(Measure measure)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update measure set name = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = measure.Id },
                    new() { Value = measure.Name }
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

    public async Task<CommandResult> DeleteMeasureAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from measure where id = $1";
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