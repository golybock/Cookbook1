using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Client;
using Cookbook.Database.Services.Interfaces.ClientInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Models.Models.Database;

namespace Cookbook.Database.Services.Client;

public class ClientFavService : IClientFavService
{
    private readonly ClientFavRepository _clientFavRepository;

    public ClientFavService()
    {
        _clientFavRepository = new ClientFavRepository();
    }
    
    public async Task<FavoriteRecipe?> GetFavoriteRecipeAsync(int id)
    {
        if (id <= 0)
            return null;
        
        return await _clientFavRepository.GetFavoriteRecipeAsync(id);
    }

    public async Task<List<FavoriteRecipe>> GetFavoriteRecipesAsync(int clientId)
    {
        if (clientId <= 0)
            return new List<FavoriteRecipe>();

        return await _clientFavRepository.GetFavoriteRecipesAsync(clientId);
    }

    public async Task<bool> GetRecipeIsLiked(int recipeId, int clientId)
    {
        if (recipeId <= 0 || clientId <= 0)
            return false;
        
        return await _clientFavRepository.GetRecipeIsLiked(recipeId, clientId);
    }

    public async Task<CommandResult> AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        if(favoriteRecipe.RecipeId <= 0 ||
           favoriteRecipe.ClientId <= 0)
            return CommandResults.BadRequest;

        return await _clientFavRepository.AddFavoriteRecipeAsync(favoriteRecipe);
    }

    public async Task<CommandResult> UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe)
    {
        if (favoriteRecipe.Id <= 0)
            return CommandResults.BadRequest;
        
        if(favoriteRecipe.RecipeId <= 0 ||
           favoriteRecipe.ClientId <= 0)
            return CommandResults.BadRequest;
        
        return await _clientFavRepository.UpdateFavoriteRecipeAsync(favoriteRecipe);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;
        
        return await _clientFavRepository.DeleteFavoriteRecipeAsync(id);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeAsync(int recipeId, int clientId)
    {
        if(recipeId <= 0 ||
           clientId <= 0)
            return CommandResults.BadRequest;
        
        return await _clientFavRepository.DeleteFavoriteRecipeAsync(recipeId, clientId);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeByRecipe(int recipeId)
    {
        if (recipeId <= 0)
            return CommandResults.BadRequest;
        
        return await _clientFavRepository.DeleteFavoriteRecipeByRecipe(recipeId);
    }

    public async Task<CommandResult> DeleteFavoriteRecipeByClient(int clientId)
    {
        if (clientId <= 0)
            return CommandResults.BadRequest;
        
        return await _clientFavRepository.DeleteFavoriteRecipeByClient(clientId);
    }
}