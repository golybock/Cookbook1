using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cookbook.Database.Services.Recipe;
using ModernWpf.Controls;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipesListView : UserControl
{
    public delegate void DeleteClick();
    public delegate void EditClick();
    public delegate void OpenClick();
    public delegate void LikeClick();
    public event DeleteClick? DeleteClicked;
    public event EditClick? EditClicked;
    public event OpenClick? OpenClicked;
    public event LikeClick? LikeClicked;

    public RecipesListView()
    {
        InitializeComponent();
    }
    
    protected virtual void OnEditClicked()
    {
        EditClicked?.Invoke();
    }

    protected virtual void OnDeleteClicked()
    {
        DeleteClicked?.Invoke();
    }

    protected virtual void OnOpenClicked()
    {
        OpenClicked?.Invoke();
    }

    protected virtual void OnLikeClicked()
    {
        LikeClicked?.Invoke();
    }

    private void RecipeMediumView_OnLikeClicked()
    {
        OnLikeClicked();
    }

    private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        OnDeleteClicked();
    }

    private void EditMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        OnEditClicked();
    }

    private void RecipeMediumView_OnClicked(object sender, MouseButtonEventArgs e)
    {
        OnOpenClicked();
    }
}