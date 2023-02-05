using Cookbook.Models.Database;
using CookbookApi.Database.Repositories.Recipe.Review;
using CookbookApi.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;
using ReviewModel = Cookbook.Models.Database.Recipe.Review.Review;

namespace CookbookApi.Database.Services.Recipe.Review;

public class ReviewService : IReviewService
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewService()
    {
        _reviewRepository = new ReviewRepository();
    }
    
    public async Task<ReviewModel> GetReviewAsync(int id)
    {
        return await _reviewRepository.GetReviewAsync(id);
    }

    public async Task<List<ReviewModel>> GetReviewsAsync(int recipeId)
    {
        return await _reviewRepository.GetReviewsAsync(recipeId);
    }

    public async Task<List<ReviewModel>> GetClientReviewAsync(int clientId)
    {
        return await _reviewRepository.GetClientReviewAsync(clientId);
    }

    public async Task<CommandResult> AddReviewAsync(ReviewModel review)
    {
        return await _reviewRepository.AddReviewAsync(review);
    }

    public async Task<CommandResult> UpdateReviewAsync(ReviewModel review)
    {
        return await _reviewRepository.UpdateReviewAsync(review);
    }

    public async Task<CommandResult> DeleteReviewAsync(int id)
    {
        return await _reviewRepository.DeleteReviewAsync(id);
    }
}