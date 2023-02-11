using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe;
using Cookbook.Database.Services.Recipe.Review;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class RecipeService : IRecipeService
{
    private readonly ClientModel _client;
    private readonly Recipe.RecipeService _recipeService;
    private readonly RecipeCategoryService _recipeCategoryService;
    private readonly RecipeImageService _recipeImageService;
    private readonly ClientFavService _clientFavService;
    private readonly RecipeStatsService _recipeStatsService;
    private readonly RecipeIngredientService _recipeIngredientService;
    private readonly ReviewService _reviewService;
    private readonly CategoryService _categoryService;

    public RecipeService(ClientModel client)
    {
        _client = client;
        _recipeService = new Recipe.RecipeService();
        _recipeCategoryService = new RecipeCategoryService();
        _recipeImageService = new RecipeImageService();
        _recipeStatsService = new RecipeStatsService();
        _recipeIngredientService = new RecipeIngredientService();
        _reviewService = new ReviewService();
        _clientFavService = new ClientFavService();
        _categoryService = new CategoryService();
    }

    public async Task<RecipeModel> GetRecipeAsync(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);
        
        var recipeStat = _recipeStatsService.GetRecipeStatsAsync(id);
        var recipeCategories = _recipeCategoryService.GetRecipeCategoriesAsync(id);
        var recipeReviews = _reviewService.GetReviewsAsync(id);
        var recipeIngredients = _recipeIngredientService.GetRecipeIngredientByRecipeAsync(id);
        var recipeRating = _reviewService.GetAvgRatingByRecipe(id);
        var category = GetRecipeMainCategoryAsync(id);
        
        if (_client.Id != 0)
        {
            var like = RecipeIsLiked(id);
            recipe.IsLiked = await like;
        }

        
        recipe.RecipeStat = await recipeStat;
        recipe.RecipeCategories = await recipeCategories;
        recipe.Reviews = await recipeReviews;
        recipe.RecipeIngredients = await recipeIngredients;
        recipe.Rating = await recipeRating;
        recipe.Category = await category;
        
        return recipe;
    }

    private Task<bool> RecipeIsLiked(int recipeId)
    {
        return _clientFavService.GetRecipeIsLiked(recipeId, _client.Id);
    }
    
    public async Task<List<RecipeModel>> GetRecipesAsync()
    {
        var recipes = await _recipeService.GetRecipesAsync();

        foreach (var recipe in recipes)
        {
            recipe.Rating =
                await _reviewService.GetAvgRatingByRecipe(recipe.Id);

            recipe.Category =
                await GetRecipeMainCategoryAsync(recipe.Id);
            
            if (_client.Id != 0)
            {
                var like = RecipeIsLiked(recipe.Id);
                recipe.IsLiked = await like;
            }
            
        }

        return recipes;
    }

    public async Task<string> GetRecipeMainCategoryAsync(int recipeId)
    {
        var recipeCategory = await _recipeCategoryService.GetRecipeMainCategoryAsync(recipeId);
        var category = await _categoryService.GetCategoryAsync(recipeCategory.Id);

        return category.Name;
    }

    public async Task<List<RecipeModel>> GetClientRecipes(int clientId)
    {
        var recipes = await _recipeService.GetClientRecipesAsync(clientId);

        foreach (var recipe in recipes)
        {
            recipe.Rating =
                await _reviewService.GetAvgRatingByRecipe(recipe.Id);

            recipe.Category =
                await GetRecipeMainCategoryAsync(recipe.Id);
        }

        return recipes;
    }
    
    public async Task<List<RecipeModel>> GetClientFavRecipes(int clientId)
    {
        List<RecipeModel> recipes = new List<RecipeModel>();
        
        var favRecipes = await _clientFavService.GetFavoriteRecipesAsync(clientId);
        
        foreach (var favRecipe in favRecipes)
        {
            var recipe = await GetRecipeAsync(favRecipe.RecipeId);
            recipes.Add(recipe);
        }

        return recipes;
    }

    public async Task<CommandResult> AddRecipeAsync(RecipeModel recipe)
    {
        CommandResult commandResult = await _recipeService.AddRecipeAsync(recipe);
        
        if (commandResult.Code == 100)
        {
            if (commandResult.Value is RecipeModel outRecipe)
            {
                var recipeStats = AddRecipeStatsAsync(outRecipe);
                var recipeIngredients = AddRecipeIngredientsAsync(outRecipe);
                var recipeCategories = AddRecipeCategoriesAsync(outRecipe);
                var recipeImages = AddRecipeImagesAsync(outRecipe);

                await recipeImages;
                await recipeStats;
                await recipeIngredients;
                await recipeCategories;
                
                commandResult = CommandResults.Successfully;
            }
            else
            {
                commandResult = CommandResults.BadRequest;
            }

            return commandResult;
        }
        commandResult = CommandResults.BadRequest;
        
        return commandResult;
    }

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        return await _recipeService.UpdateRecipeAsync(recipe);
    }


    private string? CopyImageToDocuments(string? path, int recipeId)
    {
        string documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";

        string outPath = $"{documentsPath}recipe_{recipeId}_{GetTimeStamp()}";

        if (File.Exists(path))
        {
            File.Copy(path, outPath);
            return outPath;
        }

        return null;
    }

    private string GetTimeStamp()
    {
        return Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
    }

    private async Task AddRecipeImagesAsync(RecipeModel recipe)
    {
        if (recipe.RecipeImages.Count > 0)
        {
            foreach (var image in recipe.RecipeImages)
            {
                try
                {
                    image.ImagePath = CopyImageToDocuments(image.ImagePath, recipe.Id);
                    await _recipeImageService.AddRecipeImageAsync(image);
                }
                catch
                {
                    // skip
                }

            }
        }
        
    }
    private async Task AddRecipeCategoriesAsync(RecipeModel recipe)
    {
        if (recipe.RecipeCategories.Count > 0)
        {
            foreach (var category in recipe.RecipeCategories)
            {
                await _recipeCategoryService.AddRecipeCategoryAsync(category);
            }
        }
    }

    private async Task AddRecipeIngredientsAsync(RecipeModel recipe)
    {
        if (recipe.RecipeIngredients.Count > 0)
        {
            foreach (var ingredient in recipe.RecipeIngredients)
            {
                await _recipeIngredientService.AddRecipeIngredientAsync(ingredient);
            }
        }
    }

    private async Task AddRecipeStatsAsync(RecipeModel recipe)
    {
        if (recipe.RecipeStat != null)
        {
            await _recipeStatsService.AddRecipeStatsAsync(recipe.RecipeStat);
        }
    }

    public async Task<CommandResult> DeleteRecipeAsync(int id)
    {
        CommandResult result;

        try
        {
            var delReviews = DeleteRecipeReviews(id);
            var delStats = DeleteRecipeStats(id);
            var delCategories = DeleteRecipeCategories(id);
            var delImages = DeleteRecipeImages(id);
            var delFavorites = DeleteFavRecipes(id, _client.Id);
            
            await delReviews;
            await delStats;
            await delCategories;
            await delImages;
            await delFavorites;
            
            result = CommandResults.Successfully;
        }
        catch(Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
        }
        
        return result;
    }

    public async Task DeleteFavRecipes(int recipeId, int clientId)
    {
        await _clientFavService.DeleteFavoriteRecipeAsync(recipeId, clientId);
    }
    
    // переписать под фунцию репозитория
    private async Task DeleteRecipeImages(int recipeId)
    {
        var recipeImages = await _recipeImageService.GetRecipeImagesAsync(recipeId);
        foreach (var recipeImage in recipeImages)
        {
            await _recipeImageService.DeleteRecipeImageAsync(recipeImage.Id);
        }
    }

    private async Task DeleteRecipeStats(int recipeId)
    {
        await _recipeStatsService.DeleteRecipeStatsAsync(recipeId);
    }

    // переписать под фунцию репозитория
    private async Task DeleteRecipeCategories(int recipeId)
    {
        var recipeCategories = await _recipeCategoryService.GetRecipeCategoriesAsync(recipeId);
        foreach (var recipeCategory in recipeCategories)
        {
            await _recipeCategoryService.DeleteRecipeCategoryAsync(recipeCategory.Id);
        }
    }
    
    // переписать под фунцию репозитория
    private async Task DeleteRecipeReviews(int recipeId)
    {
        var recipeReviews = await _reviewService.GetReviewsAsync(recipeId);

        foreach (var review in recipeReviews)
        {
            await _reviewService.DeleteReviewAsync(review.Id);
        }
    }

    public async Task<CommandResult> AddRecipeToFav(FavoriteRecipe favoriteRecipe)
    {
        return await _clientFavService.AddFavoriteRecipeAsync(favoriteRecipe);
    }
}