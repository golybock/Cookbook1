using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using Npgsql;
using ReviewModel = Cookbook.Models.Database.Recipe.Review.Review;

namespace Cookbook.Database.Repositories.Recipe.Review;

public class ReviewRepository : MainDbClass, IReviewRepository
{
    public async Task<ReviewModel> GetReviewAsync(int id)
    {
        connection.Open();
        try
        {
            ReviewModel review = new ReviewModel();
            string query = $"select * from review where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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
            await connection.CloseAsync();
        }
    }

    public async Task<List<ReviewModel>> GetReviewsAsync(int recipeId)
    {
        connection.Open();
        try
        {
            List<ReviewModel> reviews = new List<ReviewModel>();
            string query = $"select * from review where recipe_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<CommandResult> AddReviewAsync(ReviewModel review)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into review(recipe_id, client_id, grade, description, is_anonymous) values ($1, $2, $3, $4, $5) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
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

    public async Task<CommandResult> UpdateReviewAsync(ReviewModel review)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update review set grade = $2, description = $3, is_anonymous = $4 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = review.Id },
                    new() { Value = review.Grade },
                    new() { Value = review.Description },
                    new() { Value = review.IsAnonymous }
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

    public async Task<CommandResult> DeleteReviewAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from review where id = $1";
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