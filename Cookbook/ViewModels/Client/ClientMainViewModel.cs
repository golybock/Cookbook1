using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Pages.Profile;
using ModernWpf.Controls;
using ClientModel = Models.Models.Database.Client.Client;
using ClientService = Cookbook.Database.Services.Client.ClientService;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Client;

public class ClientMainViewModel : INotifyPropertyChanged
{
    public ClientMainViewModel(ClientModel client, Frame frame)
    {
        Frame = frame;

        Client = client;
        
        _recipesViewService = new RecipesViewService(Client);
        _clientService = new ClientService();
        _recipeService = new RecipeService(Client);
        Recipes = new List<RecipeModel>();

        LoadClientRecipes();
    }
    
    // основные модели
    public ClientModel Client { get; set; }

    public List<RecipeModel> Recipes
    {
        get => Client.Recipes;
        set
        {
            Client.Recipes = value;
            OnPropertyChanged();
        }
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
        _recipesViewService.OpenClicked(id, Recipes, Frame.NavigationService);
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
        _recipesViewService.EditClicked(id, Recipes, Frame.NavigationService);
        OnPropertyChanged("Recipes");
    }
    
    
    // приватные атрибуты
    private Frame Frame { get; set; } // навигация на страницу редактирования
    private readonly RecipeService _recipeService; // получение списка рецептов
    private readonly RecipesViewService _recipesViewService;
    private readonly ClientService _clientService;
    
    public CommandHandler EditClientCommand =>
        new CommandHandler(OnEditClient);
    
    private async void LoadClientRecipes() =>
        Recipes = await _recipeService.GetClientRecipes(Client.Id);
    
    private void OnEditClient()
    {
        if (Frame.NavigationService != null)
            Frame.NavigationService.Navigate(
                new EditProfilePage(Client, Frame)
            );
    }

    // Реализация inotify
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