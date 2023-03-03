using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe;
using Cookbook.Database.Services.Recipe.Ingredients;
using Cookbook.Database.Services.Recipe.Review;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe.Ingredients;
using Models.Models.Database;
using Models.Models.Database.Recipe;
using Models.Models.Database.Recipe.Ingredients;
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
    private readonly IngredientService _ingredientService;
    private readonly RecipeTypeService _recipeTypeService;
    private readonly MeasureService _measureService;

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
        _ingredientService = new IngredientService();
        _recipeTypeService = new RecipeTypeService();
        _measureService = new MeasureService();
    }

    public async Task<RecipeModel> GetRecipeAsync(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);
        
        var recipeStat = _recipeStatsService.GetRecipeStatsAsync(id);
        var recipeReviews = _reviewService.GetReviewsAsync(id);
        var recipeIngredients = _recipeIngredientService.GetRecipeIngredientByRecipeAsync(id);
        var category = GetRecipeMainCategoryAsync(id);
         
        if (_client.Id != 0)
        {
            var like = RecipeIsLiked(id);
            recipe.IsLiked = await like;
        }

        
        recipe.RecipeStat = await recipeStat;
        recipe.Reviews = await recipeReviews;
        recipe.RecipeIngredients = await recipeIngredients;
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
            recipe.Category =
                await GetRecipeMainCategoryAsync(recipe.Id);
            
            
            if (_client.Id != 0)
            {
                var like = RecipeIsLiked(recipe.Id);
                recipe.IsLiked = await like;
            }
            
            foreach (var ingredient in recipe.RecipeIngredients)
            {
                ingredient.Ingredient =
                    await _ingredientService.GetIngredientAsync(ingredient.IngredientId);

            }
        }

        return recipes;
    }

    public Task<List<Category>> GetCategoriesAsync()
    {
        return _categoryService.GetCategories();;
    }

    public Task<List<Ingredient>> GetIngredientsAsync()
    {
        return _ingredientService.GetIngredientsAsync();
    }

    public Task<List<RecipeType>> GetRecipeTypes()
    {
        return _recipeTypeService.GetRecipeTypesAsync();
    }

    private async Task<Category> GetRecipeMainCategoryAsync(int recipeId)
    {
        var recipeCategory = await _recipeCategoryService.GetRecipeMainCategoryAsync(recipeId);
        var category = await _categoryService.GetCategoryAsync(recipeCategory.CategoryId);

        return category;
    }

    public async Task<List<RecipeModel>> GetClientRecipes(int clientId)
    {
        var recipes = await _recipeService.GetClientRecipesAsync(clientId);

        foreach (var recipe in recipes)
        {
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
    
    public async Task<List<RecipeModel>> GetClientFavRecipes(int clientId)
    {
        List<RecipeModel> recipes = new List<RecipeModel>();
        
        var favRecipes = await _clientFavService.GetFavoriteRecipesAsync(clientId);
        
        foreach (var favRecipe in favRecipes)
        {
            var recipe = await GetRecipeAsync(favRecipe.RecipeId);
            
            recipe.Category =
                await GetRecipeMainCategoryAsync(recipe.Id);
            
            if (_client.Id != 0)
            {
                var like = RecipeIsLiked(recipe.Id);
                recipe.IsLiked = await like;
            }
            
            recipes.Add(recipe);
        }

        return recipes;
    }

    public async Task<List<RecipeModel>> FindRecipesAsync(string searchString)
    {
        List<RecipeModel> recipes = await GetRecipesAsync();

        searchString = searchString.ToLower();
        
        var firstFind =
            recipes.Where(c => c.Name.ToLower().Contains(searchString) || 
                               c.Id.ToString().Contains(searchString))
            .ToList();

        var secondFind =
            recipes.Where(c => c.Category != null &&
                                            c.Category.Name
                                                .ToLower()
                                                .Contains(searchString))
            .ToList();

        var thirdFind =
            recipes.Where(c => c.Description != null &&
                                           c.Description
                                               .ToLower()
                                               .Contains(searchString))
            .ToList();

        var result = firstFind.Concat(secondFind);
        
        result = result.Concat(thirdFind);
        result = result.Distinct();
        
        return result.ToList();


    }

    public async Task<CommandResult> AddRecipeAsync(RecipeModel? recipe)
    {
        CommandResult commandResult = await _recipeService.AddRecipeAsync(recipe);
        
        if (commandResult.Code == 100)
        {
            if (commandResult.Value is RecipeModel outRecipe)
            {
                var recipeStats = AddRecipeStatsAsync(outRecipe);
                var recipeIngredients = AddRecipeIngredientsAsync(outRecipe);
                // var recipeCategories = AddRecipeCategoriesAsync(outRecipe);
                var recipeImages = AddRecipeImagesAsync(outRecipe);
                var recipeText = WriteRecipeFileAsync(outRecipe);
                var recipeImage = AddRecipeImageAsync(outRecipe);

                await recipeImages;
                await recipeStats;
                await recipeIngredients;
                // await recipeCategories;
                await recipeText;
                await recipeImage;
                
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

    private async Task<string> WriteRecipeFileAsync(RecipeModel recipe)
    {
        string documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";

        string filePath = $"recipe_{recipe.Id}_{GetTimeStamp()}";

        string outPath = documentsPath + filePath + ".txt";

        await using (StreamWriter sw = new StreamWriter(outPath))
        {
            await sw.WriteAsync(recipe.Text);
        }

        return filePath;
    }

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        CommandResult commandResult = await _recipeService.UpdateRecipeAsync(recipe);
        
                
        if (commandResult.Code == 100)
        {
            if (commandResult.Value is RecipeModel outRecipe)
            {
                var recipeStats = _recipeStatsService.UpdateRecipeStatsAsync(recipe.RecipeStat);
                var recipeIngredients = AddRecipeIngredientsAsync(outRecipe);
                // var recipeCategory = AddRecipeCategoryAsync(outRecipe);
                var recipeImages = AddRecipeImagesAsync(outRecipe);

                await recipeImages;
                await recipeStats;
                await recipeIngredients;
                // await recipeCategory;
                
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


    private string? CopyImageToDocuments(string? path, int recipeId)
    {
        string documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";

        string filePath = $"recipe_{recipeId}_{GetTimeStamp()}.png";

        string writePath = documentsPath + filePath;

        if (File.Exists(path))
        {
            File.Copy(path, writePath);
            return filePath;
        }

        return null;
    }

    private string GetTimeStamp()
    {
        return
            Convert.ToString(
                (int)DateTime.
                    UtcNow.
                    Subtract(new DateTime(1970, 1, 1)).
                    TotalSeconds);
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
    
    private async Task AddRecipeImageAsync(RecipeModel recipe)
    {
        recipe.RecipeImage.RecipeId = recipe.Id;
        
        recipe.RecipeImage.ImagePath =
            CopyImageToDocuments(recipe.RecipeImage.ImagePath, recipe.Id);
        
        await _recipeImageService.AddRecipeImageAsync(recipe.RecipeImage);
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

    public async Task<List<Measure>> GetMeasures()
    {
        return await _measureService.GetMeasuresAsync();
    }
    
    public async Task<CommandResult> AddRecipeToFav(FavoriteRecipe favoriteRecipe)
    {
        return await _clientFavService.AddFavoriteRecipeAsync(favoriteRecipe);
    }

    public async Task<CommandResult> AddRecipeTypeAsync(RecipeType recipeType)
    {
        return await _recipeTypeService.AddRecipeTypeAsync(recipeType);
    }

    public async Task<CommandResult> AddRecipeCategoryAsync(Category category)
    {
        return await _categoryService.AddCategoryAsync(category);
    }

    public async Task<CommandResult> AddIngredient(Ingredient ingredient)
    {
        return await _ingredientService.AddIngredientAsync(ingredient);
    }
}