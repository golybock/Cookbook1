using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Views.Auth;

public partial class RegistrationView : UserControl
{
    public RegistrationView() =>
        InitializeComponent();
    
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