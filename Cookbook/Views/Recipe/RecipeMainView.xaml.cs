using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cookbook.Views.Recipe;

public partial class RecipeMainView : UserControl
{
    public delegate void DeleteClick(int id);
    public delegate void EditClick(int id);
    public delegate void LikeCLick(int id);
    public event DeleteClick? DeleteClicked;
    public event EditClick? EditClicked;
    public event LikeCLick? LikeClicked;

    public RecipeMainView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke(Int32.Parse(Id.Text));
    }

    protected virtual void OnEditClicked(int id)
    {
        EditClicked?.Invoke(id);
    }

    protected virtual void OnDeleteClicked(int id)
    {
        DeleteClicked?.Invoke(id);
    }
    
    
}