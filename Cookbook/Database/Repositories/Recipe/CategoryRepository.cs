using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class CategoryRepository : RepositoryBase, ICategoryRepository
{
    public async Task<Category> GetCategoryAsync(int id)
    {
        var category = new Category();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from category where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters = {new NpgsqlParameter {Value = id}}
            };

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                category.Name = reader.GetString(reader.GetOrdinal("name"));
            }

            return category;
        }
        catch
        {
            return new Category();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = new List<Category>();

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "select * from category";

            await using var cmd = new NpgsqlCommand(query, con);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var category = new Category();

                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                category.Name = reader.GetString(reader.GetOrdinal("name"));

                categories.Add(category);
            }

            return categories;
        }
        catch
        {
            return new List<Category>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddCategoryAsync(Category category)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "insert into category(name) values ($1) returning id";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = category.Name}
                }
            };

            result = CommandResults.Successfully;

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) category.Id = reader.GetInt32(reader.GetOrdinal("id"));

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

    public async Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        CommandResult result;

        var con = GetConnection();

        try
        {
            con.Open();

            var query = "update category set name = $2 where id = $1";

            await using var cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = category.Id},
                    new NpgsqlParameter {Value = category.Name}
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

    public async Task<CommandResult> DeleteCategoryAsync(int id)
    {
        return await DeleteAsync("category", id);
    }
}