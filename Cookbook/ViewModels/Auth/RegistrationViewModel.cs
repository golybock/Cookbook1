using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Pages;
using Microsoft.Win32;
using Models.Models.Database.Client;
using Models.Models.Register;
using ModernWpf.Controls;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.ViewModels.Auth;

public sealed class RegistrationViewModel : INotifyPropertyChanged
{
    public RegistrationViewModel(Frame frame)
    {
        OnCreated();
        
        _registerService = new RegisterService();
        _firstFrame = frame;
        
        // создаем объект
        Client = new Client();
    }

    public RegistrationViewModel(string login, Frame frame)
    {
        OnCreated();
        
        _registerService = new RegisterService();
        _firstFrame = frame;

        // создаем объект
        Client = new Client();
        
        // задаем логи по умолчанию
        Login = login;
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

    // приватные атрибуты
    private bool _loginValid;
    private bool _passwordValid;
    private string _error = null!;
    
    private RegisterService _registerService;
    private Frame _firstFrame;
    
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);
    
    public CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

    public CommandHandler CancelCommand =>
        new CommandHandler(CancelRegistration);

    public CommandHandler RegisterCommand =>
        new CommandHandler(Registration);

    public bool HasError =>
        !string.IsNullOrEmpty(Error);
    
    // приватные короткие функции
    private void CancelRegistration() =>
        ShowAcceptDialog();
    
    private void SetImage(string path) =>
        ImagePath = path;
    
    private void ShowError(string error) =>
        Error = error;
    
    private void SetLoginInvalid() =>
        LoginValid = false;

    private void SetPasswordInvalid() =>
        PasswordValid = false;
    
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
                    string file = files[0];
                
                    if (file.EndsWith(".png") || file.EndsWith(".jpg"))
                        SetImage(file);
                }
            }
        }
    }

    private async void ShowAcceptDialog()
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Отмена регистрации",
            Content = "Вы уверены, что хотите отменить регистрацию?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
            _firstFrame
                .NavigationService?
                .GoBack();
    }

    private async void Registration()
    {
        RegisterResult result = await _registerService.Register(Client);

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
        string dir = "C:\\";
        string filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
        
        // открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog();

        openFileDialog.InitialDirectory = dir;
        openFileDialog.Filter = filter;

            // если показан
        if (openFileDialog.ShowDialog() == true)
            // если есть выбранный файл
            if (openFileDialog.FileName != String.Empty)
                SetImage(openFileDialog.FileName);
    }
    
    // PropertyChanged for bindings
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}