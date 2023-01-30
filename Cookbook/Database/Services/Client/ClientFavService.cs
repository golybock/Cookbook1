using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Services.Client;

public class ClientFavService : IClientFavService
{
    private readonly ClientFavRepository _clientFavRepository;

    public ClientFavService()
    {
        _clientFavRepository = new ClientFavRepository();
    }
    
    public async Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        return await _clientFavRepository.GetFavoriteRecipeAsync(id);
    }

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        return await _clientFavRepository.GetFavoriteRecipesAsync(clientId);
    }

    public async Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        return await _clientFavRepository.AddFavoriteRecipeAsync(favoriteRecipe);
    }

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        return await _clientFavRepository.UpdateFavoriteRecipeAsync(favoriteRecipe);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        return await _clientFavRepository.DeleteFavoriteRecipeAsync(id);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        return await _clientFavRepository.DeleteFavoriteRecipeByRecipe(recipeId);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        return await _clientFavRepository.DeleteFavoriteRecipeByClient(clientId);
    }
}