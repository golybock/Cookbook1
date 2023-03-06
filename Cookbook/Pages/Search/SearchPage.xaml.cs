using System.Windows.Controls;
using Cookbook.ViewModels.Search;
using Models.Models.Database.Client;
using Frame = ModernWpf.Controls.Frame;

namespace Cookbook.Pages.Search;

public partial class SearchPage : Page
{
    public SearchPage(Client client, Frame frame)
    {
        InitializeComponent();
        DataContext = new SearchViewModel(client, frame);
    }
}