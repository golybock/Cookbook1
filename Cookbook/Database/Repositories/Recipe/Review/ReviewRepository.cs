using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using Models.Models.Database;
using Npgsql;
using ReviewModel = Cookbook.Models.Database.Recipe.Review.Review;

namespace Cookbook.Database.Repositories.Recipe.Review;

public class ReviewRepository : MainDbClass, IReviewRepository
{
    public async Task<ReviewModel?> GetReviewAsync(int id)
    {
        var con = GetConnection();
        
        con.Open();
        try
        {
            ReviewModel review = new ReviewModel();
            string query = $"select * from review where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                review.Id = reader.GetInt32(reader.GetOrdinal("id"));
                review.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                review.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                review.Grade = reader.GetInt32(reader.GetOrdinal("grade"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                review.Description = description == DBNull.Value ? null : description.ToString();
                review.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("is_anonymous"));
                review.DateOfAdding = reader.GetDateTime(reader.GetOrdinal("date_of_adding"));
            }
            
            return review;
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

    public async Task<decimal> GetAvgRatingByRecipe(int recipeId)
    {
        var con = GetConnection();
        
        con.Open();
        
        decimal avg = 0;
        
        try
        {
            string query = $"select avg(grade) from review where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = recipeId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                avg = reader.GetDecimal(reader.GetOrdinal("avg"));
            }   
            
            return avg;
        }
        catch
        {
            return avg;
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ReviewModel>> GetReviewsAsync(int recipeId)
    {
        var con = GetConnection();
        List<ReviewModel> reviews = new List<ReviewModel>();
        con.Open();
        try
        {
            
            string query = $"select * from review where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = recipeId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ReviewModel review = new ReviewModel();
                review.Id = reader.GetInt32(reader.GetOrdinal("id"));
                review.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                review.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                review.Grade = reader.GetInt32(reader.GetOrdinal("grade"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                review.Description = description == DBNull.Value ? null : description.ToString();
                review.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("is_anonymous"));
                review.DateOfAdding = reader.GetDateTime(reader.GetOrdinal("date_of_adding"));
                reviews.Add(review);
            }
            
            return reviews;
        }
        catch
        {
            return new List<ReviewModel>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<List<ReviewModel>> GetClientReviewAsync(int clientId)
    {
        var con = GetConnection();
        List<ReviewModel> reviews = new List<ReviewModel>();
        con.Open();
        try
        {

            string query = $"select * from review where client_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters = { new() { Value = clientId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ReviewModel review = new ReviewModel();
                review.Id = reader.GetInt32(reader.GetOrdinal("id"));
                review.RecipeId = reader.GetInt32(reader.GetOrdinal("recipe_id"));
                review.ClientId = reader.GetInt32(reader.GetOrdinal("client_id"));
                review.Grade = reader.GetInt32(reader.GetOrdinal("grade"));
                var description = reader.GetValue(reader.GetOrdinal("description"));
                review.Description = description == DBNull.Value ? null : description.ToString();
                review.IsAnonymous = reader.GetBoolean(reader.GetOrdinal("is_anonymous"));
                review.DateOfAdding = reader.GetDateTime(reader.GetOrdinal("date_of_adding"));
                reviews.Add(review);
            }
            
            return reviews;
        }
        catch
        {
            return new List<ReviewModel>();
        }
        finally
        {
            await con.CloseAsync();
        }
    }

    public async Task<CommandResult> AddReviewAsync(ReviewModel review)
    {
        CommandResult result;
        var con = GetConnection();
        
        con.Open();
        try
        {
            string query = $"insert into review(recipe_id, client_id, grade, description, is_anonymous)" +
                           $" values ($1, $2, $3, $4, $5) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = review.RecipeId },
                    new() { Value = review.ClientId },
                    new() { Value = review.Grade },
                    new() { Value = review.Description },
                    new() { Value = review.IsAnonymous }
                }
            }; 
            
            result = CommandResults.Successfully;
            
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                result.ValueId = reader.GetInt32(reader.GetOrdinal("id"));
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

    public async Task<CommandResult> UpdateReviewAsync(ReviewModel review)
    {
        CommandResult result;
                var con = GetConnection();
        
        con.Open();
        try
        {
            string query = $"update review set grade = $2, description = $3, is_anonymous = $4 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, con)
            {
                Parameters =
                {
                    new() { Value = review.Id },
                    new() { Value = review.Grade },
                    new() { Value = review.Description },
                    new() { Value = review.IsAnonymous }
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

    public async Task<CommandResult> DeleteReviewAsync(int id)
    {
       return await DeleteAsync("review", id);
    }
}