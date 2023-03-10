using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Recipe;

public class RecipeMainViewModel
{
    private readonly Frame _frame;
    private readonly RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;

    public RecipeMainViewModel(RecipeModel recipe, ClientModel client, Frame frame)
    {
        Recipe = recipe;
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

    public RecipeModel Recipe { get; set; }

    // Команды для биндов
    public RelayCommand<int> LikeCommand =>
        new(LikeClicked);

    public RelayCommand<int> DeleteCommand =>
        new(DeleteClicked);

    public RelayCommand<int> EditCommand =>
        new(EditClicked);

    public RelayCommand<int> GenerateFileCommand =>
        new(GenerateFileClicked);

    // сами команды
    private async void LikeClicked(int id) =>
        await _recipesViewService.LikeClicked(id);

    private void GenerateFileClicked(int id) =>
        _recipesViewService.GenerateFile(id);

    private async void DeleteClicked(int id) =>
        await _recipesViewService.DeleteClicked(id);

    private void EditClicked(int id) =>
        _recipesViewService.EditClicked(Recipe, _frame.NavigationService);
}