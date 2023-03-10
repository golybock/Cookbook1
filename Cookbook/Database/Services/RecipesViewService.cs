using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Cookbook.FIleGenerating;
using Cookbook.Models.Database.Client;
using Cookbook.Pages.Recipe;
using ModernWpf.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class RecipesViewService
{
    private readonly RecipeService _recipeService;
    private readonly ClientModel _client;
    private readonly Frame _frame;
    
    public RecipesViewService(ClientModel client, Frame frame)
    {
        _frame = frame;
        
        _client = client;

        _recipeService = new RecipeService(_client);
    }

    public void LikeClicked(int id, List<RecipeModel> recipes)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe!.IsLiked == true)
        {
            recipe.IsLiked = false;
            _recipeService.DeleteFavRecipes(id);
        }
        else
        {
            recipe.IsLiked = true;
            _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = id});
        }
    }

    public async Task LikeClicked(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);

        if (recipe.IsLiked == true)
        {
            recipe.IsLiked = false;
            await _recipeService.DeleteFavRecipes(id);
        }
        else
        {
            recipe.IsLiked = true;
            await _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = id});
        }
    }
    
    public async void OpenClicked(int id, NavigationService? navigationService)
    {
        var recipe = await GetRecipe(id);

        if (navigationService != null)
            navigationService.Navigate(
                new RecipePage(recipe, _client, _frame)
            );
    }
    
    private async Task<List<RecipeModel>> ShowAcceptDialog(int id, List<RecipeModel> recipes)
    {
        if (await ShowDeleteDialog() == ContentDialogResult.Primary)
            return await DeleteRecipe(id, recipes);

        return recipes;
    }
    
    private async Task ShowAcceptDialog(int id)
    {
        if (await ShowDeleteDialog() == ContentDialogResult.Primary)
            await DeleteRecipe(id);
    }

    private async Task<ContentDialogResult> ShowDeleteDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Удаление элемента",
            Content = "Вы уверены, что хотите удалить этот рецепт?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Удалить",
            DefaultButton = ContentDialogButton.Primary
        };

        return await acceptDialog.ShowAsync();
    }
    
    private static async void ShowErrorDialog(string error)
    {
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Ошибка",
            Content = error,
            CloseButtonText = "Закрыть",
        };
        
        await addDialog.ShowAsync();
    }
    
    public async void GenerateFile(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);
        RecipeDocx.Generate(recipe);
    }
    
    private async Task<List<RecipeModel>> DeleteRecipe(int id, List<RecipeModel> recipes)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe != null)
            if (recipe.ClientId == _client.Id)
            {
                await _recipeService.DeleteRecipeInfoAsync(recipe.Id);
                await _recipeService.DeleteRecipe(recipe.Id);
                recipes.Remove(recipe);
            }
            else
                ShowErrorDialog("Рецепт принадлежит не вам!");

        return recipes;
    }
    
    private async Task DeleteRecipe(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);

        if (recipe.Id != 0)
            if (recipe.ClientId == _client.Id)
            {
                await _recipeService.DeleteRecipeInfoAsync(recipe.Id);
                await _recipeService.DeleteRecipe(recipe.Id);
            }
            else
                ShowErrorDialog("Рецепт принадлежит не вам!");
        
        _frame.NavigationService.GoBack();
    }
    
    public void EditClicked(int id, List<RecipeModel> recipes, NavigationService? navigationService)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe.ClientId == _client.Id)
        {
            if (navigationService != null)
                if (recipe != null)
                    navigationService.Navigate(
                        new EditRecipePage(recipe, _client, _frame)
                    );            
        }
        else
            ShowErrorDialog("Рецепт принадлежит не вам!");
    }

    public async Task EditClicked(int id, NavigationService? navigationService)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);

        if (recipe.ClientId == _client.Id)
        {
            if (navigationService != null)
                if (recipe.Id != 0)
                    navigationService.Navigate(
                        new EditRecipePage(recipe, _client, _frame)
                    );
        }
        else
        {
            ShowErrorDialog("Рецепт принадлежит не вам!");   
        }
    }
    
    public async Task<List<RecipeModel>> DeleteClicked(int id, List<RecipeModel> recipes) =>
        await ShowAcceptDialog(id, recipes);

    public async Task DeleteClicked(int id) =>
        await ShowAcceptDialog(id);
    
    private Task<RecipeModel> GetRecipe(int recipeId) =>
         _recipeService.GetRecipeAsync(recipeId);
}