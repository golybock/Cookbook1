using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace CookbookApi.Database.Services.Interfaces.ClientInterfaces;

public interface IClientFavService
{
    public Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id);
    public Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId);
    public Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> DeleteFavoriteRecipeAsync(int id);
    public Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId);
    public Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId);
}