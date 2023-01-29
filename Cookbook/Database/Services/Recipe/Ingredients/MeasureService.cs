using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Database.Repositories.Recipe.Ingredients;
using Cookbook.Database.Services.Interfaces.RecipeInterfaces.IngredientsInterfaces;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Recipe.Ingredients;

namespace Cookbook.Database.Services.Recipe.Ingredients;

public class MeasureService : IMeasureService
{
    private readonly MeasureRepository _measureRepository;

    public MeasureService()
    {
        _measureRepository = new MeasureRepository();
    }
    
    public Task<Measure> GetMeasureAsync(int id)
    {
        return _measureRepository.GetMeasureAsync(id);
    }

    public Task<List<Measure>> GetMeasuresAsync()
    {
        return _measureRepository.GetMeasuresAsync();
    }

    public Task<CommandResult> AddMeasureAsync(Measure measure)
    {
        return _measureRepository.AddMeasureAsync(measure);
    }

    public Task<CommandResult> UpdateMeasureAsync(Measure measure)
    {
        return _measureRepository.UpdateMeasureAsync(measure);
    }

    public Task<CommandResult> DeleteMeasureAsync(int id)
    {
        return _measureRepository.DeleteMeasureAsync(id);
    }
}