using System;
using Cookbook.Command;
using Cookbook.Database.Services;
using ModernWpf.Controls;
using ClientModel = Models.Models.Database.Client.Client;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Recipe;

public class RecipeMainViewModel
{
    private readonly RecipesViewService _recipesViewService;
    private readonly RecipeService _recipeService;
    private readonly Frame _frame;
    public RecipeModel Recipe { get; set; }

    public RecipeMainViewModel(RecipeModel recipe, ClientModel client, Frame frame)
    {
        Recipe = recipe;
        _frame = frame;

        _recipeService = new RecipeService(client);
        _recipesViewService = new RecipesViewService(client, frame);


        _frame.Navigated += async (sender, args) =>
        {
            Recipe = await _recipeService.GetRecipeAsync(Recipe.Id);
        };
        
    }

    // Команды для биндов
    public RelayCommand<Int32> LikeCommand =>
        new RelayCommand<int>(LikeClicked);
    
    public RelayCommand<Int32> DeleteCommand =>
        new RelayCommand<int>(DeleteClicked);
    
    public RelayCommand<Int32> EditCommand =>
        new RelayCommand<int>(EditClicked);

    public RelayCommand<Int32> GenerateFileCommand =>
        new RelayCommand<int>(GenerateFileClicked);

    // сами команды
    private async void LikeClicked(int id) =>
        await _recipesViewService.LikeClicked(id);

    private void GenerateFileClicked(int id) =>
        _recipesViewService.GenerateFile(id);

    private async void DeleteClicked(int id) =>
        await _recipesViewService.DeleteClicked(id);

    private async void EditClicked(int id) =>
        await _recipesViewService.EditClicked(id, _frame.NavigationService);
}