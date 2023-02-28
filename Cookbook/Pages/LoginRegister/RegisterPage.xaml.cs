using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cookbook.Database.Services;
using Cookbook.Models.Database.Client;
using Cookbook.Models.Register;
using Microsoft.Win32;
using Client = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.LoginRegister;

public partial class RegisterPage : Page
{
    private Client _client;
    private bool _hasError;
    private ClientService _clientService;
    
    public RegisterPage()
    {
        _client = new Client();
        _clientService = new ClientService(_client);
        InitializeComponent();
    }
    
    public RegisterPage(string login)
    {
        _client = new Client() { Login = login };
        _clientService = new ClientService(_client);
        InitializeComponent();
    }

    private void RegisterPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        DataContext = _client;
    }

    private void EditImageButton_OnClick(object sender, RoutedEventArgs e)
    {
        ChooseImage();
    }

    private void PersonPicture_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ChooseImage();
    }

    private void PersonPicture_OnDrop(object sender, DragEventArgs e)
    {
        string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
        string file = files[0];
        // если файл картинка
        if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            SetImage(file);
    }

    private void ChooseImage()
    {
        // открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "Image files (*.png)|*.png|All files (*.*)|*.*";
        // если показан
        if (openFileDialog.ShowDialog() == true)
        {
            // если есть выбранный файл
            if (openFileDialog.FileName != String.Empty)
            {
                SetImage(openFileDialog.FileName);
            }
        }
    }

    private void SetImage(string path)
    {
        // сохраняем путь в объекте
        _client.ClientImage.ImagePath = path;
        _client.ClientImages.Add(new ClientImage(){ImagePath = path});
        // отображаем изображение
        PersonPicture.ProfilePicture = new BitmapImage(new Uri(_client.ClientImage.ImagePath));
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (NavigationService != null) 
            NavigationService.GoBack();
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        Register();
    }

    private async Task Register()
    {
        _client.Password = PasswordBox.Password;
        
        RegisterResult result = await _clientService.Register(_client);
        
        if (result.Result)
        {
            if (NavigationService != null) 
                NavigationService.Navigate(new NavigationPage(_client));
        }
        else
        {
            if (result.Code == 101)
            {
                InvalidLogin(result.Description);
            }
            else if (result.Code == 102)
            {
                InvalidPassword(result.PasswordResult.Description);
            }
            else if (result.Code == 103)
            {
                InvalidData(result.Description);
            }
            else
            {
                ShowError("Неизвестная ошибка");
            }
            
        }

    }
    
    private void ShowError(string error)
    {
        // показываем описание ошибки
        ErrorLabel.Visibility = Visibility.Visible;
        ErrorLabel.Text = error;
        // сохраняем статус
        _hasError = true;
    }
    
    private void SetInvalidModeToTextBox(TextBox textBox)
    {
        textBox.BorderBrush = Brushes.Red;
    }
    
    private void SetInvalidModeToPasswordBox(PasswordBox passwordBox)
    {
        passwordBox.BorderBrush = Brushes.Red;
    }
    
    private void RemoveInvalidModeFromTextBox(TextBox textBox)
    {
        textBox.BorderBrush = Brushes.Black;
    }
    
    private void RemoveInvalidModeFromPasswordBox(PasswordBox passwordBox)
    {
        passwordBox.BorderBrush = Brushes.Black;
    }

    private void InvalidLogin(string result)
    {
        SetInvalidModeToTextBox(LoginTextBox);
        ShowError(result);
    }

    private void InvalidPassword(string result)
    {
        SetInvalidModeToPasswordBox(PasswordBox);
        // очищаем пароль
        PasswordBox.Password = String.Empty;
        // выводим ошибку
        ShowError(result);
    }

    private void InvalidData(string result)
    {
        // очищаем пароль
        PasswordBox.Password = String.Empty;
        // подсвечиваем неправильные данные
        SetInvalidModeToTextBox(LoginTextBox);
        SetInvalidModeToPasswordBox(PasswordBox);
        // выводим ошибку
        ShowError(result);
    }
    
}