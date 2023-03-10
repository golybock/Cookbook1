using System.Collections.Generic;
using System.Windows.Controls;
using Cookbook.Models.Database.Recipe.Ingredients;

namespace Cookbook.ContentDialogs;

public partial class AddIngredientView : UserControl
{
    public AddIngredientView(Ingredient ingredient, List<Measure> measures)
    {
        InitializeComponent();

        MeasuresComboBox.ItemsSource = measures;
        MeasuresComboBox.SelectedIndex = 0;
        DataContext = ingredient;
    }
}