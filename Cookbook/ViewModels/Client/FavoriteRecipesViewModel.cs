using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using ModernWpf.Controls;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;
using ClientModel = Cookbook.Models.Database.Client.Client;
using RecipeService = Cookbook.Database.Services.Recipe;

namespace Cookbook.ViewModels.Client;

public class FavoriteRecipesViewModel : INotifyPropertyChanged
{
    private readonly ClientModel _client;
    private readonly Frame _frame;
    private readonly Database.Services.RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    private bool _nothingShowVisability;
    private List<RecipeModel> _recipes;
    private bool _recipesVisability = true;

    public FavoriteRecipesViewModel(ClientModel client, Frame frame)
    {
        _frame = frame;
        _client = client;
        
        _recipeService = new Database.Services.RecipeService(client);
        _recipesViewService = new RecipesViewService(client, frame);
        Recipes = new List<RecipeModel>();

        _frame = frame;

        _frame.NavigationService.Navigated += NavigationServiceOnNavigated;

        GetRecipes();
    }
    
    public void Deconstruct() =>
        _frame.NavigationService.Navigated -= NavigationServiceOnNavigated;

    private void NavigationServiceOnNavigated(object sender, NavigationEventArgs e) =>
        GetRecipes();

    public List<RecipeModel> Recipes
    {
        get => _recipes;
        set
        {
            if (Equals(value, _recipes)) return;
            _recipes = value;
            OnPropertyChanged();
        }
    }

    public bool NothingShowVisability
    {
        get => _nothingShowVisability = false;
        set
        {
            if (value == _nothingShowVisability) return;
            _nothingShowVisability = value;
            OnPropertyChanged();
        }
    }

    public bool RecipesVisability
    {
        get => _recipesVisability = true;
        set
        {
            if (value == _recipesVisability) return;
            _recipesVisability = value;
            _nothingShowVisability = !value;
            OnPropertyChanged();
        }
    }


    // Команды для биндов
    public RelayCommand<int> OpenCommand => new(OpenClicked);

    public RelayCommand<int> LikeCommand => new(LikeClicked);

    public RelayCommand<int> DeleteCommand => new(DeleteClicked);

    public RelayCommand<int> EditCommand => new(EditClicked);

    public RelayCommand<int> PrintCommand => new(GenerateFileClicked);

    private async Task GetRecipes()
    {
        Recipes = await _recipeService.GetClientFavRecipes(_client.Id);
        if (Recipes.Count > 0)
            RecipesVisability = true;
    }

    // сами команды
    private void OpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, _frame.NavigationService);
        OnPropertyChanged("Recipes");
    }

    private void LikeClicked(int id)
    {
        _recipesViewService.LikeClicked(id, Recipes);
        OnPropertyChanged("Recipes");
    }

    private void DeleteClicked(int id)
    {
        _recipesViewService.DeleteClicked(id, Recipes);
        OnPropertyChanged("Recipes");
    }

    private void EditClicked(int id)
    {
        _recipesViewService.EditClicked(id, Recipes, _frame.NavigationService);
        OnPropertyChanged("Recipes");
    }

    private void GenerateFileClicked(int obj)
    {
        _recipesViewService.GenerateFile(obj);
    }

    // реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}