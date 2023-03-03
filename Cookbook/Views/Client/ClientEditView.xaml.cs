using System.Windows;
using System.Windows.Controls;

namespace Cookbook.Views.Client;

public partial class ClientEditView : UserControl
{
    public ClientEditView()
    {
        InitializeComponent();
    }
    
    private void DataChanged(object sender, RoutedEventArgs e) =>
        DataChanged();

    private void DataChanged()
    {
        if (DataContext != null)
            if (((dynamic) DataContext).HasError)
                ((dynamic) DataContext).Error
                    = string.Empty;
    }
}