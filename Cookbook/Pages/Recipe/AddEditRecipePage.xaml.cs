using System.Windows;
using System.Windows.Controls;

namespace Cookbook.Pages.Recipe;

public partial class AddEditRecipePage : Page
{
    // private CookbookContext _context = new CookbookContext();
    private Models.Database.Recipe.Recipe _recipe;
    
    public AddEditRecipePage()
    {
        InitializeComponent();
        _recipe = new Models.Database.Recipe.Recipe();
        DataContext = _recipe;
    }
    
    public AddEditRecipePage(Models.Database.Recipe.Recipe recipe)
    {
        InitializeComponent();
        _recipe = recipe;
        DataContext = _recipe;
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        // RefreshInput();
        //
        // try
        // {
        //     _recipe.ClientId = 1;
        //     _recipe.RecipeTypeId = 1;
        //     _context.Recipes.Add(_recipe);
        //     _context.SaveChanges();
        //     _recipe = new Recipe();
        //     DataContext = _recipe;
        // }
        // catch (Exception exception)
        // {
        //     string error = Validation.GetErrors(NameTextBox)[0].ErrorContent.ToString();
        //     OutError(error);
        // }
    }

    private void OutError(string error)
    {
        ErrorTextBlock.Text = error;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }

    private void ClearError()
    {
        ErrorTextBlock.Text = null;
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }
    
    private void RefreshInput()
    {
        NameTextBox.Text = NameTextBox.Text;
        TimeNumberBox.Value = TimeNumberBox.Value;
        PortionsNumberBox.Value = PortionsNumberBox.Value;
    }

    private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ClearError();
    }
}