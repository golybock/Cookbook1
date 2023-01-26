using System.Collections.Generic;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientFavoriteRepository
{
    public FavoriteRecipe GetFavoriteRecipeAsync(int id);
    public List<FavoriteRecipe> GetFavoriteRecipesAsync(int clientId);
    public CommandResult AddFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public CommandResult UpdateFavoriteRecipeAsync(FavoriteRecipe favoriteRecipe);
    public CommandResult DeleteFavoriteClientAsync(int id);
}