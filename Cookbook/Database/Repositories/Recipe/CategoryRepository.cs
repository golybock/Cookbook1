﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe;

public class CategoryRepository : MainDbClass, ICategoryRepository
{
    public async Task<Category> GetCategoryAsync(int id)
    {
        connection.Open();
        try
        {
            Category category = new Category();
            string query = $"select * from category where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        connection.Open();
        List<Category> categories = new List<Category>();
        try
        {
            string query = $"select * from category";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                Category category = new Category();
                category.Id = reader.GetInt32(reader.GetOrdinal("id"));
                category.Name = reader.GetString(reader.GetOrdinal("name"));
            }
            
            return categories;
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

    public async Task<CommandResult> AddCategoryAsync(Category category)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into category(name) values ($1) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = category.Name },
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

    public async Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update category set name = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = category.Id },
                    new() { Value = category.Name }
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

    public async Task<CommandResult> DeleteCategoryAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from category where id = $1";
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