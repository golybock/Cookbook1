using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Review;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using Models.Models.Database;
using ModernWpf.Controls;
using ReviewModel = Cookbook.Models.Database.Recipe.Review.Review;

namespace Cookbook.Database.Services.Recipe.Review;

public class ReviewService : IReviewService
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewService()
    {
        _reviewRepository = new ReviewRepository();
    }
    
    public async Task<ReviewModel?> GetReviewAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _reviewRepository.GetReviewAsync(id);
    }

    public async Task<decimal> GetAvgRatingByRecipe(int recipeId)
    {
        if (recipeId <= 0)
            return -1;
        
        return await _reviewRepository.GetAvgRatingByRecipe(recipeId);
    }

    public async Task<List<ReviewModel>> GetReviewsAsync(int recipeId)
    {
        if (recipeId <= 0)
            return new List<ReviewModel>();
        
        return await _reviewRepository.GetReviewsAsync(recipeId);
    }

    public async Task<List<ReviewModel>> GetClientReviewAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<ReviewModel>();
        
        return await _reviewRepository.GetClientReviewAsync(clientId);
    }

    public async Task<CommandResult> AddReviewAsync(ReviewModel review)
    {
        if(review.Grade <= 0 || review.Grade > 5)
            return CommandResults.BadRequest;
        
        if(review.ClientId <= 0)
            return CommandResults.BadRequest;
        
        if(review.RecipeId <= 0)
            return CommandResults.BadRequest;

        return await _reviewRepository.AddReviewAsync(review);
    }

    public async Task<CommandResult> UpdateReviewAsync(ReviewModel review)
    {
        if(review.Grade <= 0 || review.Grade > 5)
            return CommandResults.BadRequest;
        
        if(review.ClientId <= 0)
            return CommandResults.BadRequest;
        
        if(review.RecipeId <= 0)
            return CommandResults.BadRequest;
        
        return await _reviewRepository.UpdateReviewAsync(review);
    }

    public async Task<CommandResult> DeleteReviewAsync(int id)
    {
        if(id <= 0)
            return CommandResults.BadRequest; 
        
        return await _reviewRepository.DeleteReviewAsync(id);
    }
}