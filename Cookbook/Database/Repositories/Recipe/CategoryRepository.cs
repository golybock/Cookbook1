using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database.Recipe;
using Models.Models.Database;
using Models.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class CategoryRepository : MainDbClass, ICategoryRepository
{
    public async Task<Category> GetCategoryAsync(int id)
    {
        var con = GetConnection();
        con.Open();
        Category category = new Category();
        try
        {
            string query = $"select * from category where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                category.Name = reader.GetString(reader.GetOrdinal("name"));
            }
            
            return category;
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

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var con = GetConnection();
        
        con.Open();
        
        List<Category> categories = new List<Category>();
        try
        {
            string query = $"select * from category";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                Category category = new Category();
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
        
        con.Open();
        try
        {
            string query = $"insert into category(name) values ($1) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = category.Name },
                }
            }; 
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        CommandResult result;
        var con = GetConnection();
        
        con.Open();
        try
        {
            string query = $"update category set name = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = category.Id },
                    new() { Value = category.Name }
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

    public async Task<CommandResult> DeleteCategoryAsync(int id)
    {
        return await DeleteAsync("category", id);
    }
    
}