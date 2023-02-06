using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.Views.Recipe;

public partial class RecipeMediumView : UserControl
{

    public RecipeMediumView()
    {
        InitializeComponent();
    }

    // private void LikeButton_OnMouseDown(object sender, MouseButtonEventArgs e)
    // {
    //     if (_recipe != null)
    //         if (_recipe.IsLiked == false)
    //         {
    //             _recipe.IsLiked = true;
    //             LikeButton.Background = Brushes.PaleVioletRed;
    //         }
    //         else
    //         {
    //             _recipe.IsLiked = false;
    //             LikeButton.Background = Brushes.White;
    //         }
    //             
    // }
}