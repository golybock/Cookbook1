using System;
using System.Collections.Generic;
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
    
    private string _sort;
    private string _search;
    private Category _filterCategory;
    
    public List<Category> Categories { get; set; } = null!;
    public List<RecipeModel> Recipes { get; set; } = null!;

    public FindPage()
    {
        _client = new ClientModel();
        _recipeService = new RecipeService(new ClientModel());
        
        LoadData();
        
        InitializeComponent();
    }
    
    public FindPage(ClientModel client)
    {
        _client = client;
        _recipeService = new RecipeService(_client);
        
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
    }

    private void FilterListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedType = FilterListView.SelectedItem as Category;
        FilterButtonTextBlock.Text = selectedType?.Name;
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

    private void RecipeMainView_OnOpenClickeded(int id)
    {
        throw new NotImplementedException();
    }

    private void RecipesListView_OnEditClicked(int id)
    {
        throw new NotImplementedException();
    }

    private void RecipesListView_OnOpenClicked(int id)
    {
        throw new NotImplementedException();
    }

    private void RecipesListView_OnLikeClicked(int id)
    {
        throw new NotImplementedException();
    }
}