using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cookbook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            CreateFolderForImages();
            CreateFolderForClientImages();
            CreateFolderForRecipeImages();
        }

        private void CreateFolderForImages()
        {
            string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images";
            
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                // nothing
            }
        }

        private void CreateFolderForRecipeImages()
        {
            string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes";
            
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                // nothing
            }
        }

        private void CreateFolderForClientImages()
        {
            string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients";
            
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                // nothing
            }
        }
        
    }
}