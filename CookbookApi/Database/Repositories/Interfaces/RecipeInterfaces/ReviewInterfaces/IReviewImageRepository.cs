using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;

namespace CookbookApi.Database.Repositories.Interfaces.RecipeInterfaces.ReviewInterfaces;

public interface IReviewImageRepository
{
    public Task<ReviewImage> GetReviewImageAsync(int id);
    public Task<List<ReviewImage>> GetReviewImagesAsync(int reviewId);
    public Task<CommandResult> AddReviewImageAsync(ReviewImage reviewImage);
    public Task<CommandResult> UpdateReviewImageAsync(ReviewImage reviewImage);
    public Task<CommandResult> DeleteReviewImageAsync(int id);

}