using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Views.Auth;

public partial class RegistrationView : UserControl
{
    private bool _hasError = false;

    public RegistrationView() =>
        InitializeComponent();

    private void SetBorderBrushRed(Border border) =>
        border.BorderBrush = Brushes.Red;
    
    private void SetBorderBrushTransparent(Border border) =>
        border.BorderBrush = Brushes.Transparent;
    
    private void InvalidLogin() =>
        SetBorderBrushRed(LoginBorder);
    
    private void DataChanged(object sender, RoutedEventArgs e) =>
        DataChanged();

    private void ValidLogin() =>
        SetBorderBrushTransparent(LoginBorder);

    private void ValidPassword() =>
        SetBorderBrushTransparent(PasswordBorder);
    
    private void InvalidPassword()
    {
        SetBorderBrushRed(PasswordBorder);
        // очищаем пароль
        PasswordBox.Password = String.Empty;
    }
    
    public void ShowError(string error)
    {
        // показываем описание ошибки
        ErrorLabel.Visibility = Visibility.Visible;
        ErrorLabel.Text = error;
        
        // сохраняем статус
        _hasError = true;
    }

    public void ShowError(string error, bool inLogin = false, bool inPassword = false)
    {
        ShowError(error);
        
        if(inLogin)
            InvalidLogin();
        
        else if(inPassword)
            InvalidPassword();
    }

    private void DataChanged()
    {
        if (_hasError)
        {
            _hasError = false;
            ValidLogin();
            ValidPassword();
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