using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;

namespace CookbookApi.Database.Services.Interfaces.RecipeInterfaces.IngredientsInterfaces;

public interface IMeasureService
{
    public Task<Measure> GetMeasureAsync(int id);
    public Task<List<Measure>> GetMeasuresAsync();
    public Task<CommandResult> AddMeasureAsync(Measure measure);
    public Task<CommandResult> UpdateMeasureAsync(Measure measure);
    public Task<CommandResult> DeleteMeasureAsync(int id);
}