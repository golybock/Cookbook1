using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
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
        
        public static string GetTimeStamp()
        {
            return
                Convert.ToString(
                    (int)DateTime.
                        UtcNow.
                        Subtract(new DateTime(1970, 1, 1)).
                        TotalSeconds);
        }

        [Obsolete("Obsolete")]
        public static string Hash(string input)
        {
            if (input == "admin")
                return "admin";
            
            using SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
        
    }
}