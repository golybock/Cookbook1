using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Cookbook.Command;
using Cookbook.Database.Services.Client;
using Microsoft.Win32;
using ModernWpf.Controls;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.ViewModels.Client;

public class ClientEditViewModel : INotifyPropertyChanged
{
    private readonly ClientService _clientService;
    private readonly Frame Frame;

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

    public CommandHandler CancelCommand => new(ShowAcceptDialog);

    public CommandHandler SaveCommand => new(SaveClient);

    public RelayCommand<DragEventArgs> DropCommand => new(OnDrop);

    public CommandHandler EditImageCommand => new(ChooseImage);


    // для биндингов
    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetImage(string path)
    {
        ImagePath = path;
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
            Title = "Отмена редактирования",
            Content = "Вы уверены, что хотите отменить изменения?",
            CloseButtonText = "Нет, отмена",
            PrimaryButtonText = "Да, отменить",
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await acceptDialog.ShowAsync();

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