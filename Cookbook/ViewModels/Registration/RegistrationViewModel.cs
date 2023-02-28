﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Cookbook.Command;
using Cookbook.Models.Database.Client;
using Microsoft.Win32;
using Models.Models.Database.Client;
using ModernWpf.Controls;

namespace Cookbook.ViewModels.Registration;

public sealed class RegistrationViewModel : INotifyPropertyChanged
{
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);
    
    private void OnDrop(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop) ?? Array.Empty<string>();

            if (files.Length > 0)
            {
                string file = files[0];
                
                if (file.EndsWith(".png") || file.EndsWith(".jpg"))
                    SetImage(file);
            }
        }
    }
    
    
    public RegistrationViewModel() { }

    public RegistrationViewModel(string login) =>
        Client.Login = login;

    public RegistrationViewModel(string login, NavigationService navigationService)
    {
        Client.Login = login; 
        _navigationService = navigationService;
    }

    
    private NavigationService? _navigationService;
    
    // public DropCommandHandler DropImageCommand(DragEventArgs e) =>
    //     new DropCommandHandler(PersonPicture_OnDrop, e);
    
    public CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

    public CommandHandler CancelCommand =>
        new CommandHandler(CancelRegistration);

    public CommandHandler RegisterCommand =>
        new CommandHandler(Registration);
    
    public Client Client { get; set; } = new Client();

    // private void PersonPicture_OnDrop(DragEventArgs e)
    // {
    //     string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
    //     string file = files[0];
    //     // если файл картинка
    //     if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //         SetImage(file);
    // }

    private void CancelRegistration() =>
        _navigationService?.GoBack();

    private void Registration()
    {
        
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