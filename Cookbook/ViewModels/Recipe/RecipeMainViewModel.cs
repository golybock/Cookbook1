using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Recipe;

public class RecipeMainViewModel : INotifyPropertyChanged
{
    private readonly Frame _frame;
    private readonly RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    private RecipeModel _recipe;

    public RecipeMainViewModel(RecipeModel recipe, ClientModel client, Frame frame)
    {
        _recipe = recipe;
        _frame = frame;

        _recipeService = new RecipeService(client);
        _recipesViewService = new RecipesViewService(client, frame);

        _frame.Navigated += FrameOnNavigated;
    }

    private async void FrameOnNavigated(object sender, NavigationEventArgs e) =>
        Recipe = await _recipeService.GetRecipeAsync(Recipe.Id);
    
    public void Deconstruct()
    {
        _frame.Navigated -= FrameOnNavigated;
    }

    public RecipeModel Recipe
    {
        get => _recipe;
        set
        {
            if (Equals(value, _recipe)) return;
            _recipe = value;
            OnPropertyChanged();
        }
    }

    // Команды для биндов
    public RelayCommand<int> LikeCommand =>
        new(LikeClicked);

    public RelayCommand<int> DeleteCommand =>
        new(DeleteClicked);

    public RelayCommand<int> EditCommand =>
        new(EditClicked);

    public RelayCommand<int> GenerateFileCommand =>
        new(GenerateFileClicked);

    public CommandHandler BackCommand =>
        new CommandHandler(OnBack);

    private void OnBack()
    {
        _frame.GoBack();
    }

    // сами команды
    private async void LikeClicked(int id) =>
        await _recipesViewService.LikeClicked(id);

    private void GenerateFileClicked(int id) =>
        _recipesViewService.GenerateFile(id);

    private async void DeleteClicked(int id) =>
        await _recipesViewService.DeleteClicked(id);

    private void EditClicked(int id) =>
        _recipesViewService.EditClicked(Recipe, _frame.NavigationService);

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