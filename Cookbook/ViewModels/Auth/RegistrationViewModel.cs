using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Models.Register;
using Cookbook.Pages;
using Microsoft.Win32;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Auth;

public sealed class RegistrationViewModel : INotifyPropertyChanged
{
    private string _error = null!;
    private readonly Frame _firstFrame;

    // приватные атрибуты
    private bool _loginValid;
    private bool _passwordValid;

    private readonly RegisterService _registerService;

    public RegistrationViewModel(Frame frame)
    {
        OnCreated();

        _registerService = new RegisterService();
        _firstFrame = frame;

        // создаем объект
        Client = new ClientModel();
    }

    public RegistrationViewModel(string login, Frame frame)
    {
        OnCreated();

        _registerService = new RegisterService();
        _firstFrame = frame;

        // создаем объект
        Client = new ClientModel();

        // задаем логи по умолчанию
        Login = login;
    }

    // основная модель данных
    private ClientModel Client { get; }

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

    public string Name
    {
        get => Client.Name ?? string.Empty;
        set
        {
            Client.Name = value;
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

    public string? ImagePath
    {
        get => Client.NewImagePath;
        set
        {
            Client.NewImagePath = value;
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

    public RelayCommand<DragEventArgs> DropCommand => new(OnDrop);

    public CommandHandler EditImageCommand => new(ChooseImage);

    public CommandHandler CancelCommand => new(CancelRegistration);

    public CommandHandler RegisterCommand => new(Registration);

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
    private void CancelRegistration()
    {
        ShowAcceptDialog();
    }

    private void SetImage(string path)
    {
        ImagePath = path;
    }

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

    // основные функции
    private void OnDrop(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var data = e.Data.GetData(DataFormats.FileDrop);

            if (data != null)
            {
                var files = (string[]) data;

                if (files.Length > 0)
                {
                    var file = files[0];

                    if (file.EndsWith(".png") || file.EndsWith(".jpg"))
                        SetImage(file);
                }
            }
        }
    }

    private async void ShowAcceptDialog()
    {
        var acceptDialog = new ContentDialog
        {
            Title = "Отмена регистрации",
            Content = "Вы уверены, что хотите отменить регистрацию?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await acceptDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
            _firstFrame
                .NavigationService?
                .GoBack();
    }

    private async void Registration()
    {
        var result = await _registerService.Register(Client);

        if (result.Result)
            SuccessfullyRegistration();

        else
            ValidateError(result);
    }

    private void ValidateError(RegisterResult result)
    {
        if (result.Code == 101)
        {
            SetLoginInvalid();

            if (result.Description != null)
                ShowError(result.Description);
        }
        else if (result.Code == 102)
        {
            SetPasswordInvalid();

            if (result.PasswordResult?.Description != null)
                ShowError(result.PasswordResult.Description);
        }
        else if (result.Code == 103)
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

    private void SuccessfullyRegistration()
    {
        if (_firstFrame.NavigationService != null)
            _firstFrame
                .NavigationService
                .Navigate(
                    new NavigationPage(Client, _firstFrame)
                );
    }

    private void ChooseImage()
    {
        var dir = "C:\\";
        var filter = "Image files (*.png)|*.png|All files (*.*)|*.*";

        // открываем диалог выбора файла
        var openFileDialog = new OpenFileDialog();

        openFileDialog.InitialDirectory = dir;
        openFileDialog.Filter = filter;

        // если показан
        if (openFileDialog.ShowDialog() == true)
            // если есть выбранный файл
            if (openFileDialog.FileName != string.Empty)
                SetImage(openFileDialog.FileName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}