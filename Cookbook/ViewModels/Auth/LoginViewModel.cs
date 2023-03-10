using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Models.Login;
using Cookbook.Pages;
using Cookbook.Pages.Auth;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Auth;

public class LoginViewModel : INotifyPropertyChanged
{
    private string _error = null!;
    private readonly Frame _firstFrame;

    private readonly LoginService _loginService;

    // приватные атрибуты
    private bool _loginValid;
    private bool _passwordValid;

    public LoginViewModel(Frame frame)
    {
        _loginService = new LoginService();
        Client = new ClientModel();

        _firstFrame = frame;

        OnCreated();
    }

    // основная модель данных
    private ClientModel Client { get; set; }

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
    public CommandHandler LoginCommand => new(OnLogin);

    public CommandHandler GuestCommand => new(OnGuest);

    public CommandHandler RegisterCommand => new(OnRegistration);

    public bool HasError =>
        !string.IsNullOrEmpty(Error);

    // PropertyChanged for bindings
    public event PropertyChangedEventHandler? PropertyChanged;

    // задаем дефолтные значения при инициализации 
    private void OnCreated()
    {
        // логин и пароль валидны при создании в любом случае
        LoginValid = true;
        PasswordValid = true;
    }

    // приватные короткие функции
    private void ShowError(string error)
    {
        Error = error;
    }

    private void SetLoginInvalid()
    {
        LoginValid = false;
    }

    private void SetPasswordInvalid()
    {
        PasswordValid = false;
    }

    private async void OnLogin()
    {
        var result = await _loginService.Login(Login, Password);

        if (result.Result)
        {
            if (result.Client != null)
                Client = result.Client;

            SuccessfullyLogin();
        }

        else
        {
            ValidateError(result);
        }
    }

    private void OnGuest()
    {
        var guest = new ClientModel {Id = -1, Name = "Гость"};

        if (_firstFrame.NavigationService != null)
            _firstFrame
                .NavigationService
                .Navigate(
                    new NavigationPage(guest, _firstFrame)
                );
    }

    private void OnRegistration()
    {
        if (Login != string.Empty)
        {
            if (_firstFrame.NavigationService != null)
                _firstFrame
                    .NavigationService
                    .Navigate(
                        new RegistrationPage(Login, _firstFrame)
                    );
        }

        else if (_firstFrame.NavigationService != null)
        {
            _firstFrame
                .NavigationService
                .Navigate(
                    new RegistrationPage(Login, _firstFrame)
                );
        }
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