using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Converters;
using Cookbook.Database.Services.Client;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Register;
using Cookbook.Pages;
using Microsoft.Win32;
using Models.Models.Database.Client;
using ModernWpf.Controls;

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
        _clientService = new ClientService();
        Client = new Client {Login = login};
    }

    public RegistrationViewModel(string login, NavigationService navigationService)
    {
        _clientService = new ClientService();
        Client = new Client{ Login = login };
        _navigationService = navigationService;
    }
    
    public string SecurePassword { get; set; }

    // основная модель данных
    public Client Client { get; set; }
    
    // атрибуты для привязки
    public bool LoginValid { get; set; }
    public bool PasswordValid { get; set; }
    public string Error { get; set; } = String.Empty;
    public bool HasError =>
        !string.IsNullOrEmpty(Error);
    
    // приватные атрибуты
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

    private void CancelRegistration() =>
        _navigationService?.GoBack();

    
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
        Client.Password = SecurePassword;
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

    private void SetImage(string path) =>
        Client.NewImagePath = path;

    
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