using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Database.Services;
using Cookbook.Models.Register;
using Microsoft.Win32;
using Models.Models.Database.Client;
using PdfSharp.Pdf.IO;

namespace Cookbook.ViewModels.Registration;

public sealed class RegistrationViewModel : INotifyPropertyChanged
{
    public RegistrationViewModel()
    {
        _clientService = new ClientService();
        Client = new Client();
    }

    public RegistrationViewModel(string login)
    {
        OnCreated();
        
        _clientService = new ClientService();
        Client = new Client {Login = login};
    }

    public RegistrationViewModel(string login, NavigationService navigationService)
    {
        OnCreated();
        
        _clientService = new ClientService();
        Client = new Client{ Login = login };
        _navigationService = navigationService;
    }

    // задаем дефолтные значения при инициализации 
    private void OnCreated()
    {
        // логин и пароль валидны при создании в любом случае
        LoginValid = true;
        PasswordValid = true;
    }

    // основная модель данных
    private Client Client { get; }
    
    // данные для приязки
    public string Password 
    { 
        get => _password;
        set 
        { 
            _password = value;
            Client.Password = _password;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            Client.Name = _name;
            OnPropertyChanged();
        }
    }

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            Client.Login = _login;
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
    
    public bool HasError =>
        !string.IsNullOrEmpty(Error);
    
    // приватные атрибуты
    private string _password = null!;
    private string _name = null!;
    private string _login = null!;
    private bool _loginValid;
    private bool _passwordValid;
    private string _error = null!;
    
    private ClientService _clientService;
    private NavigationService? _navigationService;
    
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);
    
    public CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

    public CommandHandler CancelCommand =>
        new CommandHandler(CancelRegistration);

    public CommandHandler RegisterCommand =>
        new CommandHandler(Registration);

    // приватные короткие функции
    private void CancelRegistration() =>
        _navigationService?.GoBack();

    private void SetImage(string path) =>
        Client.NewImagePath = path;
    
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
    
    // private async Task Register()
    // {
    //     // Client.Password = PasswordBox.Password;
    //     //
    //     // RegisterResult result = await _clientService.Register(_client);
    //     //
    //     // if (result.Result)
    //     // {
    //     //     if (NavigationService != null) 
    //     //         NavigationService.Navigate(new NavigationPage(_client));
    //     // }
    //     // else
    //     // {
    //     //     if (result.Code == 101)
    //     //     {
    //     //         InvalidLogin(result.Description);
    //     //     }
    //     //     else if (result.Code == 102)
    //     //     {
    //     //         InvalidPassword(result.PasswordResult.Description);
    //     //     }
    //     //     else if (result.Code == 103)
    //     //     {
    //     //         InvalidData(result.Description);
    //     //     }
    //     //     else
    //     //     {
    //     //         ShowError("Неизвестная ошибка");
    //     //     }
    //     //     
    //     // }
    //
    // }


    private async void Registration()
    {
        Error = "aboba";
        LoginValid = true;

        // RegisterResult result = await _clientService.Register(Client);
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