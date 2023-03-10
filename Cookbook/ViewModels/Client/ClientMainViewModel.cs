using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Database.Services.Client;
using Cookbook.Pages.Profile;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Client;

public class ClientMainViewModel : INotifyPropertyChanged
{
    private readonly ClientService _clientService;
    private readonly RecipeService _recipeService; // получение списка рецептов
    private readonly RecipesViewService _recipesViewService;

    public ClientMainViewModel(ClientModel client, Frame frame)
    {
        Frame = frame;

        Client = client;

        _recipesViewService = new RecipesViewService(Client, frame);
        _clientService = new ClientService();
        _recipeService = new RecipeService(Client);
        Recipes = new List<RecipeModel>();

        Frame.NavigationService.Navigated += NavigationServiceOnNavigated;
        
        LoadClientRecipes();
    }

    public void Deconstruct() =>
        Frame.NavigationService.Navigated -= NavigationServiceOnNavigated;

    
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

    private void NavigationServiceOnNavigated(object sender, NavigationEventArgs e) => LoadClientRecipes();
    
    // Команды для биндов
    public RelayCommand<int> OpenCommand => new(OpenClicked);

    public RelayCommand<int> LikeCommand => new(LikeClicked);

    public RelayCommand<int> DeleteCommand => new(DeleteClicked);

    public RelayCommand<int> EditCommand => new(EditClicked);

    public RelayCommand<int> PrintCommand => new(GenerateFileClicked);

    // приватные атрибуты
    private Frame Frame { get; } // навигация на страницу редактирования

    public CommandHandler EditClientCommand => new(OnEditClient);

    // Реализация inotify
    public event PropertyChangedEventHandler? PropertyChanged;

    // сами команды
    private void OpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, Frame.NavigationService);
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

    private void GenerateFileClicked(int obj)
    {
        _recipesViewService.GenerateFile(obj);
    }

    private async void LoadClientRecipes()
    {
        Recipes = await _recipeService.GetClientRecipes(Client.Id);
    }

    private void OnEditClient()
    {
        if (Frame.NavigationService != null)
            Frame.NavigationService.Navigate(
                new EditProfilePage(Client, Frame)
            );
    }

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