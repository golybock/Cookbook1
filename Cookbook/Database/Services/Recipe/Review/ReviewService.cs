using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Review;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;
using Cookbook.Models.Database;
using ReviewModel = Cookbook.Models.Database.Recipe.Review.Review;

namespace Cookbook.Database.Services.Recipe.Review;

public class ReviewService : IReviewService
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewService()
    {
        _reviewRepository = new ReviewRepository();
    }
    
    public Task<ReviewModel> GetReviewAsync(int id)
    {
        return _reviewRepository.GetReviewAsync(id);
    }

    public Task<List<ReviewModel>> GetReviewsAsync(int recipeId)
    {
        return _reviewRepository.GetReviewsAsync(recipeId);
    }

    public Task<CommandResult> AddReviewAsync(ReviewModel review)
    {
        return _reviewRepository.AddReviewAsync(review);
    }

    public Task<CommandResult> UpdateReviewAsync(ReviewModel review)
    {
        return _reviewRepository.UpdateReviewAsync(review);
    }

    public Task<CommandResult> DeleteReviewAsync(int id)
    {
        return _reviewRepository.DeleteReviewAsync(id);
    }
}