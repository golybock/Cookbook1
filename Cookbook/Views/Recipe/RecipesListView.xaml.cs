using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipesListView : UserControl
{
    public delegate void DeleteClick(int id);
    public delegate void EditClick(int id);
    public delegate void OpenClick(int id);
    public delegate void LikeCLick(int id);
    public event DeleteClick? DeleteClicked;
    public event EditClick? EditClicked;
    public event OpenClick? OpenClicked;
    public event LikeCLick? LikeClicked;

    public RecipesListView()
    {
        InitializeComponent();
    }

    private void RecipeMediumView_OnLikeClicked(int id)
    {
        LikeClicked?.Invoke(id);
    }

    private void RecipeMediumView_OnDeleteClicked(int id)
    {
        DeleteClicked?.Invoke(id);
    }

    private void RecipeMediumView_OnEditClicked(int id)
    {
        EditClicked?.Invoke(id);
    }

    private void RecipeMediumView_OnOpenClicked(int id)
    {
        OpenClicked?.Invoke(id);
    }
}