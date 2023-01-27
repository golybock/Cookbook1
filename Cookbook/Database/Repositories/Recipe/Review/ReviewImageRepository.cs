﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;
using Npgsql;

namespace Cookbook.Database.Repositories.Recipe.Review;

public class ReviewImageRepository : MainDbClass, IReviewImageRepository
{
    public async Task<ReviewImage> GetReviewImageAsync(int id)
    {
        connection.Open();
        try
        {
            ReviewImage reviewImage = new ReviewImage();
            string query = $"select * from review_images where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = id} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                reviewImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                reviewImage.ImagePath = reader.GetString(reader.GetOrdinal("iamge_path"));
                reviewImage.ReviewId = reader.GetInt32(reader.GetOrdinal("review_id"));
            }
            
            return reviewImage;
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

    public async Task<List<ReviewImage>> GetReviewImagesAsync(int reviewId)
    {
        connection.Open();
        try
        {
            List<ReviewImage> reviewImages = new List<ReviewImage>();
            string query = $"select * from review_images where review_id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters = { new() { Value = reviewId} }
            };
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                ReviewImage reviewImage = new ReviewImage();
                reviewImage.Id = reader.GetInt32(reader.GetOrdinal("id"));
                reviewImage.ImagePath = reader.GetString(reader.GetOrdinal("iamge_path"));
                reviewImage.ReviewId = reader.GetInt32(reader.GetOrdinal("review_id"));
                reviewImages.Add(reviewImage);
            }
            
            return reviewImages;
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

    public async Task<CommandResult> AddReviewImageAsync(ReviewImage reviewImage)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"insert into review_images(review_id, image_path) values ($1, $2) returning id";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = reviewImage.ReviewId },
                    new() { Value = reviewImage.ImagePath }
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

    public async Task<CommandResult> UpdateReviewImageAsync(ReviewImage reviewImage)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"update review_images set image_path = $2 where id = $1";
            await using NpgsqlCommand cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new() { Value = reviewImage.Id },
                    new() { Value = reviewImage.ImagePath }
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

    public async Task<CommandResult> DeleteReviewImageAsync(int id)
    {
        CommandResult result;
        connection.Open();
        try
        {
            string query = $"delete from review_images where id = $1";
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