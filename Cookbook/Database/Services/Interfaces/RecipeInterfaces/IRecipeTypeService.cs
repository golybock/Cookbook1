using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Models.Database;
using Models.Models.Database.Recipe;

namespace Cookbook.Database.Services.Interfaces.RecipeInterfaces;

public interface IRecipeTypeService
{
    public Task<RecipeType?> GetRecipeTypeAsync(int id);
    public Task<List<RecipeType>> GetRecipeTypesAsync();
    public Task<CommandResult> AddRecipeTypeAsync(RecipeType recipeType);
    // public Task<CommandResult> UpdateRecipeTypeAsync(RecipeModel recipe);
    // public Task<CommandResult> DeleteRecipeTypeAsync(int id);
}