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
    
    public Task<FavoriteRecipe> GetFavoriteRecipeAsync(int id)
    {
        return _clientFavRepository.GetFavoriteRecipeAsync(id);
    }

    public Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        return _clientFavRepository.GetFavoriteRecipesAsync(clientId);
    }

    public Task<bool> GetRecipeIsLiked(int recipeId, int clientId)
    {
        return _clientFavRepository.GetRecipeIsLiked(recipeId, clientId);
    }

    public Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        return  _clientFavRepository.AddFavoriteRecipeAsync(favoriteRecipe);
    }

    public Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        return _clientFavRepository.UpdateFavoriteRecipeAsync(favoriteRecipe);
    }

    public Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        return _clientFavRepository.DeleteFavoriteRecipeAsync(id);
    }

    public Task<CommandResult> DeleteFavoriteRecipeAsync(int recipeId, int clientId)
    {
        return _clientFavRepository.DeleteFavoriteRecipeAsync(recipeId, clientId);
    }

    public Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        return _clientFavRepository.DeleteFavoriteRecipeByRecipe(recipeId);
    }

    public Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        return _clientFavRepository.DeleteFavoriteRecipeByClient(clientId);
    }
}