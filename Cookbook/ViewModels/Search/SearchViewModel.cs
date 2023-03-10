using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cookbook.Command;
using Cookbook.Database.Services;
using Models.Models.Database.Recipe;
using SortType =  Models.Models.InterfacesExtensions.SortType;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using ClientModel = Models.Models.Database.Client.Client;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.ViewModels.Search;

public class SearchViewModel : INotifyPropertyChanged
{
    public SearchViewModel(ClientModel client, Frame frame)
    {
        _frame = frame;

        _selectedCategory = _emptyCategory;
        _searchString = string.Empty;

        _recipeService = new RecipeService(client);
        _recipesViewService = new RecipesViewService(client, frame);

        LoadCategories();
        GetRecipes();
    }

    public string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            
            OnPropertyChanged();
            
            ShowRecipes();
        } 
    }
    
    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            
            OnPropertyChanged();
            
            ShowRecipes();
        } 
    }
    
    public SortType? SelectedSortType
    {
        get => _selectedSortType ?? global::Models.Models.InterfacesExtensions.SortTypes.Default;
        set
        {
            _selectedSortType = value;
            
            OnPropertyChanged();
            
            ShowRecipes();
        } 
    }
    
    private string _searchString;
    private Category _selectedCategory;
    private SortType? _selectedSortType;
    
    private readonly Frame _frame;
    private readonly RecipeService _recipeService;
    private readonly RecipesViewService _recipesViewService;
    private readonly Category _emptyCategory = new() { Id = -1, Name = "Все категории" };
    
    private bool _recipesVisability = true;
    private List<Category> _categories = new List<Category>();
    private List<RecipeModel> _recipes = new List<RecipeModel>();
    
    public bool RecipesVisability
    {
        get => _recipesVisability;
        set
        {
            _recipesVisability = value;
            OnPropertyChanged();
            OnPropertyChanged("NothingShowVisability");
        }
    }

    public bool NothingShowVisability =>
        !_recipesVisability;

    public List<Category> Categories
    {
        get => _categories;
        set
        {
            if (Equals(value, _categories)) return;
            _categories = value;
            OnPropertyChanged();
        }
    }

    public List<RecipeModel> Recipes
    {
        get => _recipes;
        set
        {
            if (Equals(value, _recipes)) return;
            _recipes = value;
            OnPropertyChanged();
        } 
    }

    public List<SortType> SortTypes { get; set; } =
        global::Models.Models.InterfacesExtensions.SortTypes.SortTypesList;

    private async void LoadCategories()
    {
        // значение по умолчанию (без фильтрации)
        Categories.Add(_emptyCategory);
    
        // Загружаем все остальные категории
        Categories.AddRange(await GetCategories());
        
        OnPropertyChanged("Categories");
    }

    private async void GetRecipes() =>
        Recipes = await _recipeService.GetRecipesAsync();

    private async Task<List<Category>> GetCategories() =>
        await _recipeService.GetCategoriesAsync();
    
    private void SortRecipes()
    {
        if (SelectedSortType?.Id == 2)
            ReverseList();
    }

    private void ReverseList()
    {
        List<RecipeModel> reversedRecipes = new List<RecipeModel>();
        
        for (int i = Recipes.Count - 1; i >= 0; i--)
            reversedRecipes.Add(Recipes.ElementAt(i));

        Recipes = reversedRecipes;
    }
    
    private async Task FilterRecipes()
    {
        if (_selectedCategory.Id != -1)
        {
            Recipes = Recipes
                .Where(
                    c => c.Category.Name == _selectedCategory.Name
                ).ToList();
            
            SetRecipesVisability();

            return;
        } 
        
        await FindRecipes();
    }

    private async Task FindRecipes()
    {
        if (_searchString != "")
            Recipes = await _recipeService.FindRecipesAsync(_searchString);
        else
            Recipes = await _recipeService.GetRecipesAsync();
        
        SetRecipesVisability();
        OnPropertyChanged("Recipes");
    }

    private void SetRecipesVisability() => 
        RecipesVisability = Recipes.Count > 0;

    private async void ShowRecipes()
    {
        // получаем рецепты
        await FindRecipes();
        
        // фильтруем по категориям
        await FilterRecipes();

        // сортируем
        SortRecipes();
        
        OnPropertyChanged("Recipes");
    }

    // Команды для биндов
    public RelayCommand<Int32> OpenCommand =>
        new RelayCommand<int>(OpenClicked);
    
    public RelayCommand<Int32> LikeCommand =>
        new RelayCommand<int>(LikeClicked);
    
    public RelayCommand<Int32> DeleteCommand =>
        new RelayCommand<int>(DeleteClicked);
    
    public RelayCommand<Int32> EditCommand =>
        new RelayCommand<int>(EditClicked);
    
    public RelayCommand<Int32> PrintCommand =>
        new RelayCommand<int>(GenerateFileClicked);

    // сами команды
    private void OpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, _frame.NavigationService);
        OnPropertyChanged("Recipes");
    }

    private void LikeClicked(int id)
    {
        _recipesViewService.LikeClicked(id, Recipes);
        OnPropertyChanged("Recipes");
    }

    private void DeleteClicked(int id)
    {
        _recipesViewService.DeleteClicked(id, Recipes);
        OnPropertyChanged("Recipes");
    }

    private void GenerateFileClicked(int obj)
    {
        _recipesViewService.GenerateFile(obj);
    }
    
    private void EditClicked(int id)
    {
        _recipesViewService.EditClicked(id, Recipes, _frame.NavigationService);
        OnPropertyChanged("Recipes");
    }

    
    // реализация INotifyPropertyChanged 
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