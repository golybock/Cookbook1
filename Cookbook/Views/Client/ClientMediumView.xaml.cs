using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Views.Client;

public partial class ClientMediumView : UserControl
{
    public delegate void LikeCLick(int id);
    public delegate void OpenClick(int id);
    public event LikeCLick? LikeClicked;
    public event OpenClick? OpenClicked;
    
    public ClientMediumView()
    {
        InitializeComponent();
        LikeButton.MouseDown += OnLikeClicked;
    }

    private void OnLikeClicked(object sender, EventArgs e)
    {
        LikeClicked?.Invoke(Int32.Parse(Id.Text));
    }
    
    private void ClientMediumView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenClicked?.Invoke(Int32.Parse(Id.Text));
    }
    
}