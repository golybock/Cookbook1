using System.Collections.Generic;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;

namespace Cookbook.Database.Repositories.Interfaces.ClientInterfaces;

public interface IClientFavoriteRepository
{
    public FavoriteRecipe GetFavoriteRecipe(int id);
    public List<FavoriteRecipe> GetFavoriteRecipes(int clientId);
    public CommandResult AddFavoriteRecipe(FavoriteRecipe favoriteRecipe);
    public CommandResult UpdateFavoriteRecipe(FavoriteRecipe favoriteRecipe);
    public CommandResult DeleteFavoriteClient(int id);
}