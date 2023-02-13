using System.Threading.Tasks;
using System.Windows.Markup;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Models.Models.Database.Client;
using ModernWpf.Controls;
using Page = System.Windows.Controls.Page;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Pages.Recipe;

public partial class RecipePage : Page
{
    private readonly Client _client;
    private RecipeModel _recipe;
    private readonly RecipeService _recipeService;
    
    public RecipePage(RecipeModel recipe, Client client)
    {
        _recipe = recipe;
        _client = client;
        _recipeService = new RecipeService(_client);
        
        DataContext = _recipe;
        
        InitializeComponent();
    }
    
    public RecipePage(Client client, int recipeId)
    {
        _client = client;
        _recipeService = new RecipeService(_client);

        GetRecipe(recipeId);
        
        InitializeComponent();
    }

    private async Task GetRecipe(int recipeId)
    {
        _recipe = await _recipeService.GetRecipeAsync(recipeId);
        DataContext = _recipe;
    }

    private void RecipeMainView_OnLikeClicked(int id)
    {
        if (_recipe!.IsLiked == true)
        {
            _recipe.IsLiked = false;
            _recipeService.DeleteFavRecipes(id, _client.Id);
        }
        else
        {
            _recipe.IsLiked = true;
            _recipeService.AddRecipeToFav(new FavoriteRecipe() {ClientId = _client.Id, RecipeId = id});
        }

        DataContext = _recipe;
    }

    private void RecipeMainView_OnDeleteClicked(int id)
    {
        ShowAcceptDialog(id);
    }

    private void RecipeMainView_OnEditClicked(int id)
    {
        if (NavigationService != null)
            NavigationService.Navigate(
                new AddEditRecipePage(_recipe)
            );
    }
    
    private async void ShowAcceptDialog(int id)
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
            DeleteRecipe(id);
#pragma warning restore CS4014
        }
    }
    
    private async Task DeleteRecipe(int id)
    {
        await _recipeService.DeleteRecipeAsync(id);
        
        if (NavigationService != null)
            NavigationService.GoBack();
    }
}