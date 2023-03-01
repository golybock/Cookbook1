using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cookbook.Database.Services.Client;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Login;
using Cookbook.Pages.Auth;
using Models.Models.Login;
using Client = Models.Models.Database.Client.Client;
using ClientService = Cookbook.Database.Services.ClientService;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.LoginRegister;

public partial class LoginPage : Page
{
    private ClientService _clientService;
    private string _login;
    private string _password;
    private bool _hasError;
    private Frame FirstFrame { get; set; }
    
    public LoginPage()
    {
        _clientService = new ClientService(new Client(){Id = 0});
        InitializeComponent();
    }
    
    public LoginPage(Frame frame)
    {
        FirstFrame = frame;
        _clientService = new ClientService(new Client(){Id = 0});
        InitializeComponent();
    }

    private void GuestButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null) 
            NavigationService.Navigate(new NavigationPage(new Client(){Id = -1, Name = "Гость"}, FirstFrame));
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        Login();
    }

    private async void Login()
    {
        // получаем информацию со страницы
        GetInfo();
        
        // выполняем авторизацию
        LoginResult loginResult = await _clientService.Login(_login, _password);
        
        // проверяем результат 
        if (loginResult.Result)
        {
            if (loginResult.Client != null)
                SuccessfulLogin(loginResult.Client);
        }
        else
        {
            // сохраняем статус
            _hasError = true;
            
            ShowErrorByCode(loginResult);
        }
    }

    private void SuccessfulLogin(Client client)
    {
        // переход на основную страницу
        if (NavigationService != null) 
            NavigationService.Navigate(new NavigationPage(client, FirstFrame));

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
    
    private void SetBorderRed(Border border)
    {
        border.BorderBrush = Brushes.Red;
    }

    private void SetBorderTransparent(Border border)
    {
        border.BorderBrush = Brushes.Transparent;
    }


    private void InvalidLogin(string result)
    {
        SetBorderRed(LoginBorder);
        ShowError(result);
    }

    private void InvalidPassword(string result)
    {
        // очищаем пароль
        PasswordBox.Password = String.Empty;
        SetBorderRed(PasswordBorder);
        ShowError(result);
    }

    private void InvalidData(string result)
    {
        SetBorderRed(LoginBorder);
        SetBorderRed(PasswordBorder);
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
        SetBorderTransparent(LoginBorder);
        SetBorderTransparent(PasswordBorder);
        // убираем ошибку
        ErrorTextBlock.Visibility = Visibility.Collapsed;
        ErrorTextBlock.Text = "Место для ошибки";
    }

    private void Input(object sender, RoutedEventArgs routedEventArgs)
    {
        if (_hasError)
        {
            _hasError = false;
            HideError();
        }
    }

    private void RegistrationTextBlock_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        GetInfo();
        
        // лучше передавать frame, а не только NavigationService
        
        if(_login != String.Empty)
            if (NavigationService != null) 
                NavigationService.Navigate(new RegistrationPage(_login, FirstFrame));
            
        else if (NavigationService != null) 
            NavigationService.Navigate(new RegistrationPage(FirstFrame));
    }
}