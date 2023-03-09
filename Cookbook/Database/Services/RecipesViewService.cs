using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
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

    public async void OpenClicked(int id, List<RecipeModel> recipes, NavigationService? navigationService)
    {
        var recipe = await GetRecipe(id);

        if (navigationService != null)
            navigationService.Navigate(
                new RecipePage(recipe)
            );
    }
    
    private async Task<List<RecipeModel>> ShowAcceptDialog(int id, List<RecipeModel> recipes)
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Удаление элемента",
            Content = "Вы уверены, что хотите удалить этот рецепт?",
            CloseButtonText = "Отмена",
            PrimaryButtonText = "Удалить",
            DefaultButton = ContentDialogButton.Primary
        };
    
        if (await acceptDialog.ShowAsync() == ContentDialogResult.Primary)
        {
#pragma warning disable CS4014
            return await DeleteRecipe(id, recipes);
#pragma warning restore CS4014
        }

        return recipes;
    }
    
    private async Task<List<RecipeModel>> DeleteRecipe(int id, List<RecipeModel> recipes)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe != null)
        {
            await _recipeService.DeleteRecipeInfoAsync(recipe.Id);
            await _recipeService.DeleteRecipe(recipe.Id);
            recipes.Remove(recipe);
        }

        return recipes;
    }

    public async Task<List<RecipeModel>> DeleteClicked(int id, List<RecipeModel> recipes) =>
        await ShowAcceptDialog(id, recipes);

    public void EditClicked(int id, List<RecipeModel> recipes, NavigationService? navigationService)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (navigationService != null)
            if (recipe != null)
                navigationService.Navigate(
                    new EditRecipePage(recipe, _client, _frame)
                );
    }

    private Task<RecipeModel> GetRecipe(int recipeId) =>
         _recipeService.GetRecipeAsync(recipeId);
}