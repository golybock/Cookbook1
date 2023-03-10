using System.Windows.Controls;
using Cookbook.Models.Database.Client;
using Cookbook.ViewModels.Search;
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