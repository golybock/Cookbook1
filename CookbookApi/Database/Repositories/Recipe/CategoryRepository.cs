using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces;
using Npgsql;

namespace CookbookApi.Database.Repositories.Recipe;

public class CategoryRepository : MainDbClass, ICategoryRepository
{
    public async Task<Category> GetCategoryAsync(int id)
    {
        Connection.Open();
        try
        {
            Category category = new Category();
            string query = $"select * from category where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        Connection.Open();
        List<Category> categories = new List<Category>();
        try
        {
            string query = $"select * from category";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection);
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddCategoryAsync(Category category)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"insert into category(name) values ($1) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        CommandResult result;
        Connection.Open();
        try
        {
            string query = $"update category set name = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, Connection)
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
            await Connection.CloseAsync();
        }
    }

    public async Task<CommandResult> DeleteCategoryAsync(int id)
    {
        return await DeleteAsync("category", id);
    }
    
}