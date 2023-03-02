using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Pages;
using Cookbook.Views.Recipe;
using Models.Models.Database.Client;
using Models.Models.Login;
using ModernWpf.Controls;

namespace Cookbook.ViewModels.Auth;

public class LoginViewModel : INotifyPropertyChanged
{
    public LoginViewModel(Frame frame)
    {
        _loginService = new LoginService();
        Client = new Client();
        
        _firstFrame = frame;

        OnCreated();
    }
    
    // задаем дефолтные значения при инициализации 
    private void OnCreated()
    {
        // логин и пароль валидны при создании в любом случае
        LoginValid = true;
        PasswordValid = true;
    }
    
    // основная модель данных
    private Client Client { get; set; }
    
    // данные для приязки
    public string Password 
    { 
        get => Client.Password;
        set 
        {
            Client.Password = value;
            OnPropertyChanged();
        }
    }

    public string Login
    {
        get => Client.Login;
        set
        {
            Client.Login = value;
            OnPropertyChanged();
        }
    }
    
    public bool LoginValid
    {
        get => _loginValid;
        set
        {
            _loginValid = value;
            OnPropertyChanged();
        }
    }

    public bool PasswordValid
    {
        get => _passwordValid;
        set
        {
            _passwordValid = value;
            OnPropertyChanged();
        }
    }

    public string Error
    {
        get => _error;
        set
        {
            _error = value;
            OnPropertyChanged();
        }
    }
    
    // команды
    public CommandHandler LoginCommand =>
        new CommandHandler(OnLogin);

    public CommandHandler GuestCommand =>
        new CommandHandler(OnGuest);

    // приватные атрибуты
    private bool _loginValid;
    private bool _passwordValid;
    private string _error = null!;

    private LoginService _loginService;
    private Frame _firstFrame;
    
    // приватные короткие функции
    private void ShowError(string error) =>
        Error = error;
    
    public bool HasError =>
        !string.IsNullOrEmpty(Error);
    
    private void SetLoginInvalid() =>
        LoginValid = false;

    private void SetPasswordInvalid() =>
        PasswordValid = false;
    
    private async void OnLogin()
    {
        LoginResult result = await _loginService.Login(Login, Password);

        if (result.Result)
        {
            if (result.Client != null) 
                Client = result.Client;
            
            SuccessfullyLogin();
        }
        
        else
            ValidateError(result);
    }

    private void OnGuest()
    {
        Client guest = new Client() {Id = -1, Name = "Гость"};
        
        if (_firstFrame.NavigationService != null) 
            _firstFrame
                .NavigationService
                .Navigate(
                    new NavigationPage(guest, _firstFrame)
                );
    }

    private void ValidateError(LoginResult result)
    {
        if (result.Code == 101 || result.Code == 201)
        {
            SetLoginInvalid();
                
            if (result.Description != null)
                ShowError(result.Description);
        }
        else if (result.Code == 102 || result.Code == 202)
        {
            SetPasswordInvalid();
                
            if (result.Description != null)
                ShowError(result.Description);
        }
        else if (result.Code == 203)
        {
            SetPasswordInvalid();
            SetLoginInvalid();
        
            if (result.Description != null)
                ShowError(result.Description);
        }
        else
        {
            ShowError("Неизвестная ошибка");
        }
    }
    
    private void SuccessfullyLogin()
    {
        if (_firstFrame.NavigationService != null) 
            _firstFrame
                .NavigationService
                .Navigate(
                    new NavigationPage(Client, _firstFrame)
                );
    }
    
    // PropertyChanged for bindings
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}