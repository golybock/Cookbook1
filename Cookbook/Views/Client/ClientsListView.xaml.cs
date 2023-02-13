using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Cookbook.Views.Client;

public partial class ClientListView : UserControl
{
    public delegate void DeleteClick(int id);
    public delegate void EditClick(int id);
    public delegate void OpenClick(int id);
    public delegate void LikeCLick(int id);
    public event DeleteClick? DeleteClicked;
    public event EditClick? EditClicked;
    public event OpenClick? OpenClicked;
    public event LikeCLick? LikeClicked;
    
    public ClientListView()
    {
        InitializeComponent();
    }

    private void ClientMediumView_OnLikeClicked(int id)
    {
        LikeClicked?.Invoke(id);
    }

    private void ClientMediumView_OnEditClicked(int id)
    {
        EditClicked?.Invoke(id);
    }

    private void ClientMediumView_OnDeleteClicked(int id)
    {
        DeleteClicked?.Invoke(id);
    }

    private void ClientMediumView_OnOpenClicked(int id)
    {
        OpenClicked?.Invoke(id);
    }
}