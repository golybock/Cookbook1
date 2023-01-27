using System;
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
        throw new System.NotImplementedException();
    }

    public async Task<CommandResult> AddCategoryAsync(Category category)
    {
        throw new System.NotImplementedException();
    }

    public async Task<CommandResult> UpdateCategoryAsync(Category category)
    {
        throw new System.NotImplementedException();
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