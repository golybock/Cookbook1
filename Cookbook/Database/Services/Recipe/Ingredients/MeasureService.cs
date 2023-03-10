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

    public async Task<Measure?> GetMeasureAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _measureRepository.GetMeasureAsync(id);
    }

    public Task<List<Measure>> GetMeasuresAsync()
    {
        return _measureRepository.GetMeasuresAsync();
    }

    public async Task<CommandResult> AddMeasureAsync(Measure measure)
    {
        if (string.IsNullOrWhiteSpace(measure.Name))
            return CommandResults.BadRequest;

        return await _measureRepository.AddMeasureAsync(measure);
    }

    public async Task<CommandResult> UpdateMeasureAsync(Measure measure)
    {
        if (measure.Id <= 0)
            return CommandResults.BadRequest;

        if (string.IsNullOrWhiteSpace(measure.Name))
            return CommandResults.BadRequest;

        return await _measureRepository.UpdateMeasureAsync(measure);
    }

    public async Task<CommandResult> DeleteMeasureAsync(int id)
    {
        if (id <= 0)
            return CommandResults.BadRequest;

        return await _measureRepository.DeleteMeasureAsync(id);
    }
}