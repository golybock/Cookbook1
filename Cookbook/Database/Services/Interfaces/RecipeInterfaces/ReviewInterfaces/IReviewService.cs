using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces.ReviewInterfaces;

public interface IReviewService
{
    public Task<Review> GetReviewAsync(int id);
    public Task<List<Review>> GetReviewsAsync(int recipeId);
    public Task<CommandResult> AddReviewAsync(Review review);
    public Task<CommandResult> UpdateReviewAsync(Review review);
    public Task<CommandResult> DeleteReviewAsync(int id);
}