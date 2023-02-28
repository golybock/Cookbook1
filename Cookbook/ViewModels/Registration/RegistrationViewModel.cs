using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Cookbook.Command;
using Cookbook.Models.Database.Client;
using Microsoft.Win32;
using Models.Models.Database.Client;
using ModernWpf.Controls;

namespace Cookbook.ViewModels.Registration;

public sealed class RegistrationViewModel : INotifyPropertyChanged
{
    public RegistrationViewModel() { }

    public RegistrationViewModel(string login) =>
        Client.Login = login;

    // private DropCommandHandler DropImageCommand(DragEventArgs e) =>
    //     new DropCommandHandler(PersonPicture_OnDrop(e));
    //
    private CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

    public Client Client { get; set; } = new Client();

    // private void PersonPicture_OnDrop(DragEventArgs e)
    // {
    //     string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
    //     string file = files[0];
    //     // если файл картинка
    //     if (file.EndsWith(".png") || file.EndsWith(".jpg"))
    //         SetImage(file);
    // }
    
    private void ChooseImage()
    {
        string dir = @"C:\\";
        string filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
        
        // открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            InitialDirectory = dir,
            Filter = filter
        };

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