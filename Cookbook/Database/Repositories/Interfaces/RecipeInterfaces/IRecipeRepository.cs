using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe; 

namespace Cookbook.Database.Repositories.Interfaces.RecipeInterfaces;

public interface IRecipeRepository
{
    public Task<RecipeModel> GetRecipeAsync(int id);
    public Task<List<RecipeModel>> GetRecipesAsync();
    public Task<List<RecipeModel>> GetClientRecipesAsync(int clientId);
    public Task<CommandResult> AddRecipeAsync(RecipeModel recipe);
    public Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe);
    public Task<CommandResult> DeleteRecipeAsync(int id);
}