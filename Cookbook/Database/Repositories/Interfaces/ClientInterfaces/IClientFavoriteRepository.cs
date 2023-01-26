using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientFavoriteRepository
{
    public Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id);
    public Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId);
    public Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public Task<CommandResult> DeleteFavoriteClientAsync(int id);
}