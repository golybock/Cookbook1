using System.Collections.Generic;
using Cookbook.Command;
using Cookbook.Pages.Profile;
using ModernWpf.Controls;
using ClientModel = Models.Models.Database.Client.Client;
using RecipeModel = Models.Models.Database.Recipe.Recipe;

namespace Cookbook.ViewModels.Client;

public class ClientMainViewModel
{
    public ClientMainViewModel(ClientModel client, Frame frame)
    {
        _frame = frame;
        Client = client;
    }
    
    public ClientModel Client { get; set; }
    public List<RecipeModel> Recipes { get; set; }
    private Frame _frame { get; set; }

    public CommandHandler EditCommand =>
        new CommandHandler(OnEditClient);

    private void OnEditClient()
    {
        if (_frame.NavigationService != null)
            _frame.NavigationService.Navigate(
                new EditProfilePage(Client)
            );
    }
}