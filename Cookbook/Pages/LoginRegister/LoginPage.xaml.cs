using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cookbook.Models;
using Cookbook.Models.Database.Client;
namespace Cookbook.Pages.LoginRegister;

public partial class LoginPage : Page
{
    // private LoginService _loginService;
    private string _login;
    private string _password;
    private bool _hasError;
    
    public LoginPage()
    {
        // _loginService = new LoginService();
        InitializeComponent();
    }

    private void GuestButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new NavigationPage());
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        Login();
    }

    private void Login()
    {
        // получаем информацию со страницы
        GetInfo();
        
        // выполняем авторизацию
        // LoginResult loginResult = _loginService.Login(_login, _password);
        
        // проверяем результат 
        // if (loginResult.Result)
        // {
        //     SuccessfulLogin(loginResult.Client);
        // }
        // else
        // {
        //     // сохраняем статус
        //     _hasError = true;
        //     
        //     ShowErrorByCode(loginResult);
        // }
    }

    private void SuccessfulLogin(Client client)
    {
        // переход на основную страницу
        NavigationService.Navigate(new NavigationPage());
            
        // очищаем данные
        ClearInput();
    }

    private void ShowErrorByCode(LoginResult loginResult)
    {
        // ошибка с логином
        if(loginResult.Code == 101 || loginResult.Code == 201)
            InvalidLogin(loginResult.Description);
            
        // ошибка с паролем
        else if(loginResult.Code == 102 || loginResult.Code == 202)
            InvalidPassword(loginResult.Description);
            
        // ошибка со всеми данными
        else if(loginResult.Code == 203)
            InvalidData(loginResult.Description);
    }
    
    private void GetInfo()
    {
        _login = LoginTextBox.Text;
        _password = PasswordBox.Password;
    }
    
    private void SetInvalidModeToTextBox(TextBox textBox)
    {
        textBox.BorderBrush = Brushes.Red;
    }
    
    private void SetInvalidModeToPasswordBox(PasswordBox passwordBox)
    {
        passwordBox.BorderBrush = Brushes.Red;
    }
    
    private void RemoveInvalidModeFromTextBox(TextBox textBox)
    {
        textBox.BorderBrush = Brushes.Black;
    }
    
    private void RemoveInvalidModeFromPasswordBox(PasswordBox passwordBox)
    {
        passwordBox.BorderBrush = Brushes.Black;
    }

    private void InvalidLogin(string result)
    {
        SetInvalidModeToTextBox(LoginTextBox);
        ShowError(result);
    }

    private void InvalidPassword(string result)
    {
        SetInvalidModeToPasswordBox(PasswordBox);
        // очищаем пароль
        PasswordBox.Password = String.Empty;
        ShowError(result);
    }

    private void InvalidData(string result)
    {
        SetInvalidModeToTextBox(LoginTextBox);
        SetInvalidModeToPasswordBox(PasswordBox);
        ShowError(result);
    }

    private void ShowError(string error)
    {
        // показываем описание ошибки
        ErrorTextBlock.Visibility = Visibility.Visible;
        ErrorTextBlock.Text = error;
        // сохраняем статус
        _hasError = true;
    }

    private void ClearInput()
    {
        PasswordBox.Password = string.Empty;
        LoginTextBox.Text = string.Empty;
        HideError();
    }

    private void HideError()
    {
        // убираем выделение
        RemoveInvalidModeFromPasswordBox(PasswordBox);
        RemoveInvalidModeFromTextBox(LoginTextBox);
        // убираем ошибку
        ErrorTextBlock.Visibility = Visibility.Collapsed;
        ErrorTextBlock.Text = "Место для ошибки";
    }

    private void Input(object sender, RoutedEventArgs routedEventArgs)
    {
        if(_hasError)
            HideError();
    }

    private void RegistrationTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        GetInfo();
        
        if(_login != String.Empty)
            NavigationService.Navigate(new RegisterPage(_login));
        else
            NavigationService.Navigate(new RegisterPage());

    }
}