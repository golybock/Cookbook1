using System.Windows;
using System.Windows.Controls;

namespace Cookbook.Views.Auth;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void DataChanged(object sender, RoutedEventArgs e) =>
        DataChanged();

    private void DataChanged()
    {
        if (DataContext != null)
        {
            if (((dynamic) DataContext).HasError)
            {
                ((dynamic) DataContext).Error
                    = string.Empty;
            
                ((dynamic) DataContext).LoginValid
                    = true;
                
                ((dynamic) DataContext).PasswordValid
                    = true;
            }
        }
    }

    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
        DataChanged();
        
        if (DataContext != null)
            ((dynamic)DataContext).Password 
                = ((PasswordBox)sender).Password;
    }
}