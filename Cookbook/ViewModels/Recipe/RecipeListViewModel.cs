using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using ModernWpf.Controls;
using RecipeService = Cookbook.Database.Services.Recipe;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Recipe;

public class RecipeListViewModel : INotifyPropertyChanged
{
    private readonly Database.Services.RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;

    private readonly Frame _frame;
    private List<RecipeModel> _recipes;

    public RecipeListViewModel(ClientModel client, Frame frame)
    {
        _recipeService =
            new Database.Services.RecipeService(client);

        _recipesViewService =
            new RecipesViewService(client, frame);

        _recipes = new List<RecipeModel>();

        _frame = frame;

        _frame.NavigationService.Navigated += NavigationServiceOnNavigated;

        GetRecipes();
    }

    public void Deconstruct() =>
        _frame.NavigationService.Navigated -= NavigationServiceOnNavigated;

    // основная модель для привязки
    public List<RecipeModel> Recipes
    {
        get => _recipes;
        set
        {
            if (Equals(value, _recipes)) return;
            _recipes = value;
            
            RecipesListNotVisible = _recipes.Count < 1;
            
            OnPropertyChanged("RecipesListNotVisible");
            OnPropertyChanged();
        }
    }

    public bool RecipesListNotVisible { get; set; } = false;

    // Команды для биндов
    public RelayCommand<int> OpenCommand => new(OpenClicked);

    public RelayCommand<int> LikeCommand => new(LikeClicked);

    public RelayCommand<int> DeleteCommand => new(DeleteClicked);

    public RelayCommand<int> EditCommand => new(EditClicked);

    public RelayCommand<int> PrintCommand => new(GenerateFileClicked);
    
    private void NavigationServiceOnNavigated(object sender, NavigationEventArgs e) => GetRecipes();

    // сами команды
    private void OpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, _frame.NavigationService);

        GetRecipes();
    }

    private void LikeClicked(int id)
    {
        _recipesViewService.LikeClicked(id, Recipes);
        OnPropertyChanged("Recipes");
    }

    private void GenerateFileClicked(int obj) =>
        _recipesViewService.GenerateFile(obj);

    private async void DeleteClicked(int id) =>
        Recipes = new List<RecipeModel>(await _recipesViewService.DeleteClicked(id, Recipes));

    private void EditClicked(int id)
    {
        _recipesViewService.EditClicked(id, Recipes, _frame.NavigationService);

        GetRecipes();
    }

    private async void GetRecipes() =>
        Recipes = await _recipeService.GetRecipesAsync();


    // реализация интерфейса INotifyPropertyChanged
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