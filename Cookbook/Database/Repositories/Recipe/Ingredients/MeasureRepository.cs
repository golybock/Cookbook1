using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Ingredients;

public class MeasureRepository : RepositoryBase, IMeasureRepository
{
    public async Task<Measure> GetMeasureAsync(int id)
    {
        var measure = new Measure();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from measure where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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
        var measures = new List<Measure>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from measure";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var measure = new Measure();
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

            var query = "insert into measure(name) values ($1) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = measure.Id},
                    new NpgsqlParameter {Value = measure.Name}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) measure.Id = reader.GetInt32(reader.GetOrdinal("id"));

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

    public async Task<CommandResult> UpdateMeasureAsync(Measure measure)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            await con.OpenAsync();

            var query = "update measure set name = $2 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = measure.Id},
                    new NpgsqlParameter {Value = measure.Name}
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

    public async Task<CommandResult> DeleteMeasureAsync(int id)
    {
        return await DeleteAsync("measure", id);
    }
}