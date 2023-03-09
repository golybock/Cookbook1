using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services.Client;
using Microsoft.Win32;
using ModernWpf.Controls;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Client;

public class ClientEditViewModel : INotifyPropertyChanged
{
    public ClientEditViewModel(ClientModel client, Frame frame)
    {
        _clientService = new ClientService();
        
        Client = client;
        Frame = frame;
        
        Client.NewImagePath = Client.ClientImage.ImagePath;
        
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

    public ClientModel Client { get; set; }
    private Frame Frame;

    private ClientService _clientService;

    public CommandHandler CancelCommand =>
        new CommandHandler(ShowAcceptDialog);

    public CommandHandler SaveCommand =>
        new CommandHandler(SaveClient);
    
    public RelayCommand<DragEventArgs> DropCommand =>
        new RelayCommand<DragEventArgs>(OnDrop);
    
    public CommandHandler EditImageCommand =>
        new CommandHandler(ChooseImage);

    private void SetImage(string path) =>
        ImagePath = path;
    
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
            Title = "Отмена редактирования",
            Content = "Вы уверены, что хотите отменить изменения?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
            Frame
                .NavigationService?
                .GoBack();
    }

    private async void SaveClient()
    {
        var result = await _clientService.UpdateClientAsync(Client);

        if (result.Result)
            SuccesfullyEdit();
    }

    
    private void SuccesfullyEdit()
    {
        Frame
            .NavigationService?
            .GoBack();
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
    

    // для биндингов
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