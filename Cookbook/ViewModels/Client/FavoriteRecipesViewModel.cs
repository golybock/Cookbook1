using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Views;
using Cookbook.Views.Recipe;
using ModernWpf.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;
using RecipeService = Cookbook.Database.Services.Recipe;

namespace Cookbook.ViewModels.Client;

public class FavoriteRecipesViewModel : INotifyPropertyChanged
{
    private readonly Frame _frame;
    private readonly ClientModel _client;
    private readonly Database.Services.RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    private List<RecipeModel> _recipes;
    private bool _nothingShowVisability = false;
    private bool _recipesVisability = true;

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

    public FavoriteRecipesViewModel(ClientModel client, Frame frame)
    {
        _frame = frame;
        _client = client;
        _recipeService = new Database.Services.RecipeService(client);
        _recipesViewService = new RecipesViewService(client, frame);
        Recipes = new List<RecipeModel>();

        GetRecipes();
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

    private async Task GetRecipes()
    {
        Recipes = await _recipeService.GetClientFavRecipes(_client.Id);
        if (Recipes.Count > 0)
            RecipesVisability = true;
    }
    
    
    // Команды для биндов
    public RelayCommand<Int32> OpenCommand =>
        new RelayCommand<int>(OpenClicked);
    
    public RelayCommand<Int32> LikeCommand =>
        new RelayCommand<int>(LikeClicked);
    
    public RelayCommand<Int32> DeleteCommand =>
        new RelayCommand<int>(DeleteClicked);
    
    public RelayCommand<Int32> EditCommand =>
        new RelayCommand<int>(EditClicked);
    
    // сами команды
    private void OpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, Recipes, _frame.NavigationService);
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