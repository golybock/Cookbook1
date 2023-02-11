using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Services.Interfaces.ClientInterfaces;

public interface IClientFavService
{
    public Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id);
    public Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId);
    public Task<bool> GetRecipeIsLiked(int recipeId, int clientId);
    public Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> DeleteFavoriteRecipeAsync(int id);
    public Task<CommandResult> DeleteFavoriteRecipeAsync(int recipeId, int clientId);
    public Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId);
    public Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId);
}