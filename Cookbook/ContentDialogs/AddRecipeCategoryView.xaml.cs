using System.Windows.Controls;
using Cookbook.Models.Database.Recipe;

namespace Cookbook.ContentDialogs;

public partial class AddRecipeCategoryView : UserControl
{
    public AddRecipeCategoryView(Category category)
    {
        InitializeComponent();
        DataContext = category;
    }
}