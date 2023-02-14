using System.Windows.Controls;
using ClientModel = Models.Models.Database.Client.Client;

namespace Cookbook.Pages.Find;

public partial class FindPage : Page
{
    public FindPage()
    {
        InitializeComponent();
    }
    
    public FindPage(ClientModel client)
    {
        InitializeComponent();
    }
}