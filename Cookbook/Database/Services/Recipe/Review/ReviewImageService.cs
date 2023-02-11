﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Review;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;
using Models.Models.Database;

namespace Cookbook.Database.Services.Recipe.Review;

public class ReviewImageService : IReviewImageService
{
    private readonly ReviewImageRepository _reviewImageRepository;

    public ReviewImageService()
    {
        _reviewImageRepository = new ReviewImageRepository();
    }

    public Task<ReviewImage> GetReviewImageAsync(int id)
    {
        return _reviewImageRepository.GetReviewImageAsync(id);
    }

    public Task<List<ReviewImage>> GetReviewImagesAsync(int reviewId)
    {
        return _reviewImageRepository.GetReviewImagesAsync(reviewId);
    }

    public Task<CommandResult> AddReviewImageAsync(ReviewImage reviewImage)
    {
        return _reviewImageRepository.AddReviewImageAsync(reviewImage);
    }

    public Task<CommandResult> UpdateReviewImageAsync(ReviewImage reviewImage)
    {
        return _reviewImageRepository.UpdateReviewImageAsync(reviewImage);
    }

    public Task<CommandResult> DeleteReviewImageAsync(int id)
    {
        return _reviewImageRepository.DeleteReviewImageAsync(id);
    }
}