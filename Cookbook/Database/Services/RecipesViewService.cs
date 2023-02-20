using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
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

    public RecipesViewService(ClientModel client)
    {
        _client = client;

        _recipeService = new RecipeService(_client);
    }

    public void LikeClicked(int id, List<RecipeModel> recipes)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe!.IsLiked == true)
        {
            recipe.IsLiked = false;
            _recipeService.DeleteFavRecipes(id, _client.Id);
        }
        else
        {
            recipe.IsLiked = true;
            _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = id});
        }
    }

    public void OpenClicked(int id, List<RecipeModel> recipes, NavigationService? navigationService)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (navigationService != null)
            if (recipe != null)
                navigationService.Navigate(
                    new RecipePage(_client, id)
                );
    }
    
    private async void ShowAcceptDialog(int id, List<RecipeModel> recipes)
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
            DeleteRecipe(id, recipes);
#pragma warning restore CS4014
        }
    }
    
    private async Task DeleteRecipe(int id, List<RecipeModel> recipes)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (recipe != null)
        {
            await _recipeService.DeleteRecipeAsync(recipe.Id);
            recipes.Remove(recipe);
        }
        
    }

    public void DeleteClicked(int id, List<RecipeModel> recipes)
    {
        ShowAcceptDialog(id, recipes);
    }

    public void EditClicked(int id, List<RecipeModel> recipes, NavigationService? navigationService)
    {
        var recipe = recipes.FirstOrDefault(c => c.Id == id);

        if (navigationService != null)
            if (recipe != null)
                navigationService.Navigate(
                    new AddEditRecipePage(recipe, _client)
                );
    }
}