using System.Windows.Controls;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Views.Client;

public partial class ClientMediumView : UserControl
{
    public ClientMediumView()
    {
        InitializeComponent();
    }
    
    public ClientMediumView(ClientModel client)
    {
        DataContext = client;
        InitializeComponent();
    }
}