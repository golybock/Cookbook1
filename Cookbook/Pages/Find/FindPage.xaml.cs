 using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cookbook.Database.Services;
using Models.Models.Database.Recipe;
using ModernWpf.Controls;
using ClientModel = Models.Models.Database.Client.Client;
using RecipeModel = Models.Models.Database.Recipe.Recipe;
using Page = System.Windows.Controls.Page;

namespace Cookbook.Pages.Find;

public partial class FindPage : Page
{
    private readonly RecipeService _recipeService;
    private readonly ClientModel _client;
    private readonly RecipesViewService _recipesViewService;
    private readonly Category? _emptyCategory = new() { Id = -1, Name = "Все категории" };
    
    private string _sort;
    private string _search;
    private Category? _filterCategory;
    
    public List<Category> Categories { get; set; } = null!;
    public List<RecipeModel> Recipes { get; set; } = null!;

    public FindPage()
    {
        _client = new ClientModel();
        
        _filterCategory = _emptyCategory;
        _search = String.Empty;
        _sort = "Default";

        _recipeService = new RecipeService(_client);
        _recipesViewService = new RecipesViewService(_client);
        
        LoadData();
        
        InitializeComponent();
    }
    
    public FindPage(ClientModel client)
    {
        _client = client;
        
        _filterCategory = _emptyCategory;
        _search = String.Empty;
        _sort = "Default";
        
        _recipeService = new RecipeService(_client);
        _recipesViewService = new RecipesViewService(_client);
        
        LoadData();
        
        InitializeComponent();
    }

    private async Task LoadData()
    {
        await LoadCategories();
        await LoadRecipes();

        DataContext = this;
    }
    
    private async Task LoadCategories()
    {
        Categories = new List<Category> {new() {Id = -1, Name = "Все категории" } };

        Categories.AddRange(await _recipeService.GetCategories());
    }

    private async Task LoadRecipes()
    {
        Recipes = new List<RecipeModel>();

        Recipes.AddRange(await _recipeService.GetRecipesAsync());
    }

    private void SortListButton_OnClick(SplitButton sender, SplitButtonClickEventArgs args)
    {
        MessageBox.Show("Абоба");
    }

    private void ListStylesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedType = SortTypeListView.SelectedItem as StackPanel;
        var uri = selectedType?.Children[0] as BitmapIcon;
        SortButtonBitmapIcon.UriSource = uri?.UriSource;
        
        ShowRecipes();
    }

    private void FilterListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedType = FilterListView.SelectedItem as Category;
        FilterButtonTextBlock.Text = selectedType?.Name;
        
        ShowRecipes();
    }

    private void FilterButton_OnClick(SplitButton sender, SplitButtonClickEventArgs args)
    {
        MessageBox.Show("Абоба");
    }

    private void SortRecipes()
    {
        if(_sort == "Default")
            return;
        if(_sort == "ByUpper")
            return;
        if(_sort == "ByLower")
            Recipes.Reverse();
    }

    private async Task FilterRecipes()
    {
        if (_filterCategory.Id != -1)
        {
            Recipes = Recipes
                .Where(
                    c => c.Categories.Contains(_filterCategory)
                ).ToList();
        }
        else
        {
            await FindRecipes();
        }
    }

    private async Task FindRecipes()
    {
        if (_search != string.Empty)
            Recipes = await _recipeService.FindRecipesAsync(_search);
        else
            Recipes = await _recipeService.GetRecipesAsync();
    }

    private void CheckRecipesCount()
    {
        if (Recipes.Count < 1)
        {
            // показываем что рецептов нет
            NothingShowView.Visibility = Visibility.Visible;
            RecipesListView.Visibility = Visibility.Collapsed;
        }
    }

    private async Task ShowRecipes()
    {
        // получаем рецепты
        await FindRecipes();

        // смотрим сколько осталось
        CheckRecipesCount();
        
        // фильтруем по категориям
        await FilterRecipes();
        
        // смотрим сколько осталось
        CheckRecipesCount();
        
        // сортируем
        SortRecipes();

        DataContext = this;
    }

    private void RecipesListView_OnEditClicked(int id)
    {
        _recipesViewService.EditClicked(id, Recipes, NavigationService);
    }

    private void RecipesListView_OnOpenClicked(int id)
    {
        _recipesViewService.OpenClicked(id, Recipes, NavigationService);
    }

    private void RecipesListView_OnLikeClicked(int id)
    {
        _recipesViewService.LikeClicked(id, Recipes);
        DataContext = this;
    }

    private void RecipesListView_OnDeleteClicked(int id)
    {
        _recipesViewService.DeleteClicked(id, Recipes);
        DataContext = this;
    }

    private void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        _search = SearchBox.Text;
        ShowRecipes();
    }
}