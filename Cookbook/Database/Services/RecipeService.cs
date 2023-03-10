using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Database.Services.Client;
using Cookbook.Database.Services.Interfaces;
using Cookbook.Database.Services.Recipe;
using Cookbook.Database.Services.Recipe.Ingredients;
using Cookbook.Models.Database;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Database.Recipe;
using Cookbook.Models.Database.Recipe.Ingredients;
using RecipeModel = Cookbook.Models.Database.Recipe.Recipe;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services;

public class RecipeService : IRecipeService
{
    private readonly CategoryService _categoryService;
    private readonly ClientModel _client;
    private readonly ClientFavService _clientFavService;
    private readonly IngredientService _ingredientService;
    private readonly MeasureService _measureService;
    private readonly RecipeCategoryService _recipeCategoryService;
    private readonly RecipeImageService _recipeImageService;
    private readonly RecipeIngredientService _recipeIngredientService;
    private readonly Recipe.RecipeService _recipeService;
    private readonly RecipeStatsService _recipeStatsService;
    private readonly RecipeTypeService _recipeTypeService;

    public RecipeService(ClientModel client)
    {
        _client = client;
        _recipeService = new Recipe.RecipeService();
        _recipeCategoryService = new RecipeCategoryService();
        _recipeImageService = new RecipeImageService();
        _recipeStatsService = new RecipeStatsService();
        _recipeIngredientService = new RecipeIngredientService();
        _clientFavService = new ClientFavService();
        _categoryService = new CategoryService();
        _ingredientService = new IngredientService();
        _recipeTypeService = new RecipeTypeService();
        _measureService = new MeasureService();
    }

    public async Task<RecipeModel> GetRecipeAsync(int id)
    {
        var recipe = await _recipeService.GetRecipeAsync(id);

        await GetRecipeInfo(recipe);

        return recipe;
    }

    public async Task<List<RecipeModel>> GetRecipesAsync()
    {
        var recipes = await _recipeService.GetRecipesAsync();

        foreach (var recipe in recipes)
            await GetRecipeInfo(recipe);

        return recipes;
    }

    public async Task<List<RecipeModel>> GetClientRecipes(int clientId)
    {
        var recipes = await _recipeService.GetClientRecipesAsync(clientId);

        foreach (var recipe in recipes)
            await GetRecipeInfo(recipe);

        return recipes;
    }

    public async Task<List<RecipeModel>> GetClientFavRecipes(int clientId)
    {
        var recipes = new List<RecipeModel>();

        var favRecipes = await _clientFavService.GetFavoriteRecipesAsync(clientId);

        foreach (var favRecipe in favRecipes)
        {
            var recipe = await GetRecipeAsync(favRecipe.RecipeId);

            await GetRecipeInfo(recipe);

            recipes.Add(recipe);
        }

        return recipes;
    }

    public async Task GetRecipeInfo(RecipeModel recipe)
    {
        recipe.RecipeStat = await _recipeStatsService.GetRecipeStatsAsync(recipe.Id);

        recipe.Category = await GetRecipeMainCategoryAsync(recipe.Id);

        recipe.RecipeType = await _recipeTypeService.GetRecipeTypeAsync(recipe.Id);

        recipe.IsLiked = await RecipeIsLiked(recipe.Id);

        recipe.RecipeIngredients = await _recipeIngredientService.GetRecipeIngredientByRecipeAsync(recipe.Id);

        recipe.RecipeImage = await _recipeImageService.GetRecipeImageByRecipeAsync(recipe.Id);
        
        foreach (var ingredient in recipe.RecipeIngredients)
            ingredient.Ingredient = await _ingredientService.GetIngredientAsync(ingredient.IngredientId);
    }

    public async Task<List<RecipeModel>> FindRecipesAsync(string searchString)
    {
        var recipes = await GetRecipesAsync();

        searchString = searchString.ToLower();

        var firstFind =
            recipes.Where(c => c.Name.ToLower().Contains(searchString) ||
                               c.Id.ToString().Contains(searchString)).ToList();

        var secondFind =
            recipes.Where(c => c.Category.Name.ToLower().Contains(searchString)).ToList();

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

    public async Task<CommandResult> AddRecipeAsync(RecipeModel recipe)
    {
        var commandResult = await _recipeService.AddRecipeAsync(recipe);

        var recipeText = WriteRecipeFileAsync(recipe);
        recipe.PathToTextFile = await recipeText;

        var updateResult = await _recipeService.UpdateRecipeAsync(recipe);

        if (commandResult.Code == 100)
        {
            if (commandResult.Value is RecipeModel outRecipe)
            {
                var recipeStats = AddRecipeStatsAsync(outRecipe.RecipeStat);
                var recipeCategory = AddRecipeCategoryAsync(new RecipeCategory
                    {RecipeId = outRecipe.Id, CategoryId = outRecipe.Category.Id});
                var recipeIngredients = AddRecipeIngredientsAsync(outRecipe);
                var recipeImage = AddRecipeImageAsync(outRecipe);

                await recipeStats;
                await recipeCategory;
                await recipeIngredients;
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

    public async Task<CommandResult> UpdateRecipeAsync(RecipeModel recipe)
    {
        var recipeText = WriteRecipeFileAsync(recipe);

        recipe.PathToTextFile = await recipeText;

        var commandResult = await _recipeService.UpdateRecipeAsync(recipe);

        await DeleteRecipeInfoAsync(recipe.Id);

        if (commandResult.Code == 100)
        {
            if (commandResult.Result)
            {
                recipe.RecipeStat.RecipeId = recipe.Id;

                var recipeStats = AddRecipeStatsAsync(recipe.RecipeStat);

                var recipeCategory = AddRecipeCategoryAsync(
                    new RecipeCategory
                    {
                        RecipeId = recipe.Id, CategoryId = recipe.Category.Id
                    }
                );

                var recipeIngredients = AddRecipeIngredientsAsync(recipe);

                var recipeImage = AddRecipeImageAsync(recipe);

                await recipeStats;
                await recipeCategory;
                await recipeIngredients;
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

    public async Task<CommandResult> DeleteRecipeInfoAsync(int id)
    {
        CommandResult result;

        try
        {
            var delStats = DeleteRecipeStats(id);
            var delCategories = DeleteRecipeCategories(id);
            var delImages = DeleteRecipeImages(id);
            var delFavorites = DeleteFavRecipes(id);
            var deleteIngredients = DeleteRecipeIngredients(id);

            await delStats;
            await delCategories;
            await delImages;
            await delFavorites;
            await deleteIngredients;

            result = CommandResults.Successfully;
        }
        catch (Exception e)
        {
            result = CommandResults.BadRequest;
            result.Description = e.ToString();
        }

        return result;
    }

    private async Task<Category> GetRecipeMainCategoryAsync(int recipeId)
    {
        var recipeCategory = await _recipeCategoryService.GetRecipeMainCategoryAsync(recipeId);
        var category = await _categoryService.GetCategoryAsync(recipeCategory!.CategoryId);

        return category;
    }

    private async Task<string> WriteRecipeFileAsync(RecipeModel recipe)
    {
        var documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";

        var filePath = $"recipe_{recipe.Id}_{App.GetTimeStamp()}";

        var outPath = documentsPath + filePath + ".txt";

        await using var sw = new StreamWriter(outPath);

        await sw.WriteLineAsync(recipe.Text);

        await sw.DisposeAsync();

        return filePath;
    }

    private string? CopyImageToDocuments(RecipeImage recipeImage)
    {
        var documentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";

        var filePath = $"recipe_{recipeImage.RecipeId}_{App.GetTimeStamp()}.png";

        var writePath = documentsPath + filePath;

        if (File.Exists(recipeImage.GetImagePath()))
        {
            File.Copy(recipeImage.GetImagePath(), writePath);
            return filePath;
        }

        return null;
    }

    private async Task AddRecipeImageAsync(RecipeModel recipe)
    {
        recipe.RecipeImage.RecipeId = recipe.Id;

        recipe.RecipeImage.ImagePath =
            CopyImageToDocuments(
                new RecipeImage
                {
                    ImagePath = recipe.NewImagePath, RecipeId = recipe.Id
                }
            );

        await _recipeImageService.AddRecipeImageAsync(recipe.RecipeImage);
    }

    private async Task AddRecipeIngredientsAsync(RecipeModel recipe)
    {
        if (recipe.RecipeIngredients.Count > 0)
            foreach (var ingredient in recipe.RecipeIngredients)
            {
                ingredient.RecipeId = recipe.Id;
                await _recipeIngredientService.AddRecipeIngredientAsync(ingredient);
            }
    }

    public Task<List<Category>> GetCategoriesAsync() =>
        _categoryService.GetCategories();

    private Task<bool> RecipeIsLiked(int recipeId) =>
        _clientFavService.GetRecipeIsLiked(recipeId, _client.Id);

    public Task DeleteRecipe(int recipeId) =>
        _recipeService.DeleteRecipeAsync(recipeId);

    private Task DeleteRecipeIngredients(int recipeId) =>
        _recipeIngredientService.DeleteRecipeIngredientByRecipeAsync(recipeId);

    public Task DeleteFavRecipes(int recipeId) =>
        _clientFavService.DeleteFavoriteRecipeByRecipe(recipeId);

    private Task DeleteRecipeImages(int recipeId) =>
        _recipeImageService.DeleteRecipeImagesByRecipeAsync(recipeId);

    private Task DeleteRecipeStats(int recipeId) =>
        _recipeStatsService.DeleteRecipeStatsAsync(recipeId);

    private Task DeleteRecipeCategories(int recipeId) =>
        _recipeCategoryService.DeleteRecipeCategoriesAsync(recipeId);

    public Task<List<Ingredient>> GetIngredientsAsync() =>
        _ingredientService.GetIngredientsAsync();

    public Task<List<RecipeType>> GetRecipeTypes() =>
        _recipeTypeService.GetRecipeTypesAsync();

    public Task<List<Measure>> GetMeasures() =>
        _measureService.GetMeasuresAsync();

    private Task AddRecipeStatsAsync(RecipeStats recipeStats) =>
        _recipeStatsService.AddRecipeStatsAsync(recipeStats);

    public Task AddRecipeToFav(FavoriteRecipe favoriteRecipe) =>
        _clientFavService.AddFavoriteRecipeAsync(favoriteRecipe);

    private Task<CommandResult> AddRecipeCategoryAsync(RecipeCategory recipeCategory) =>
        _recipeCategoryService.AddRecipeCategoryAsync(recipeCategory);

    public Task<CommandResult> AddRecipeTypeAsync(RecipeType recipeType) =>
        _recipeTypeService.AddRecipeTypeAsync(recipeType);

    public Task<CommandResult> AddCategoryAsync(Category category) =>
        _categoryService.AddCategoryAsync(category);

    public Task<CommandResult> AddIngredient(Ingredient ingredient) =>
        _ingredientService.AddIngredientAsync(ingredient);
    
}