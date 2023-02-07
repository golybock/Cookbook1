using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Review;

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces.ReviewInterfaces;

public interface IReviewRepository
{
    public Task<Review> GetReviewAsync(int id);
    public Task<decimal> GetAvgRatingByRecipe(int recipeId);
    public Task<List<Review>> GetReviewsAsync(int recipeId);
    public Task<List<Review>> GetClientReviewAsync(int clientId);
    public Task<CommandResult> AddReviewAsync(Review review);
    public Task<CommandResult> UpdateReviewAsync(Review review);
    public Task<CommandResult> DeleteReviewAsync(int id);
}