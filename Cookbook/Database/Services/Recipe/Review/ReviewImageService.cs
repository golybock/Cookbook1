using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Review;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;
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

    public async Task<ReviewImage> GetReviewImageAsync(int id)
    {
        if (id <= 0)
            return null; 
        
        return await _reviewImageRepository.GetReviewImageAsync(id);
    }

    public async Task<List<ReviewImage>> GetReviewImagesAsync(int reviewId)
    {
        if (reviewId <= 0)
            return new List<ReviewImage>();
        
        return await _reviewImageRepository.GetReviewImagesAsync(reviewId);
    }

    public async Task<CommandResult> AddReviewImageAsync(ReviewImage reviewImage)
    {
        if(reviewImage.ReviewId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(reviewImage.ImagePath))
            return CommandResults.BadRequest;

        if (!File.Exists(reviewImage.ImagePath))
            return CommandResults.BadRequest;
        
        return await _reviewImageRepository.AddReviewImageAsync(reviewImage);
    }

    public async Task<CommandResult> UpdateReviewImageAsync(ReviewImage reviewImage)
    {
        if(reviewImage.Id <= 0)
            return CommandResults.BadRequest;
        
        if(reviewImage.ReviewId <= 0)
            return CommandResults.BadRequest;
        
        if(string.IsNullOrWhiteSpace(reviewImage.ImagePath))
            return CommandResults.BadRequest;

        if (!File.Exists(reviewImage.ImagePath))
            return CommandResults.BadRequest;
        
        return await _reviewImageRepository.UpdateReviewImageAsync(reviewImage);
    }

    public async Task<CommandResult> DeleteReviewImageAsync(int id)
    {
        if(id <= 0)
            return CommandResults.BadRequest;
        
        return await _reviewImageRepository.DeleteReviewImageAsync(id);
    }
}