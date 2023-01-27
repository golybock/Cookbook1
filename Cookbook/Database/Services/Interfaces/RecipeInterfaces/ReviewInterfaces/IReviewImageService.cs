using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;

public interface IReviewImageService
{
    public Task<ReviewImage> GetReviewImageAsync(int id);
    public Task<List<ReviewImage>> GetReviewImagesAsync(int reviewId);
    public Task<CommandResult> AddReviewImageAsync(ReviewImage reviewImage);
    public Task<CommandResult> UpdateReviewImageAsync(ReviewImage reviewImage);
    public Task<CommandResult> DeleteReviewImageAsync(int id);

}