using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cookbook.Command;
using Cookbook.Database.Services;
using Models.Models.Database.Client;
using Frame = ModernWpf.Controls.Frame;
using RecipeService = Cookbook.Database.Services.Recipe;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Recipe;

public class RecipeListViewModel : INotifyPropertyChanged
{
    public RecipeListViewModel(Client client, Frame frame)
    {
        _recipeService = 
            new Database.Services.RecipeService(client);
        
        _recipesViewService =
            new RecipesViewService(client);
        
        Recipes = new List<RecipeModel>();
        
        _frame = frame;

        GetRecipes();
    }
    
    // основная модель для привязки
    public List<RecipeModel> Recipes { get; set; }
    
    private Frame _frame;

    private readonly Database.Services.RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    
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

    private async void GetRecipes()
    {
        Recipes = await _recipeService.GetRecipesAsync();
        OnPropertyChanged("Recipes");
    }
    
    // реализация интерфейса iNotify
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