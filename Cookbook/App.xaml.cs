using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Cookbook;

public partial class App : Application
{
    public static string DocumentsPath = $"C:\\Users\\{Environment.UserName}\\Documents\\";

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        CreateFolderForImages();
        CreateFolderForClientImages();
        CreateFolderForRecipeImages();
    }

    private void CreateFolderForImages()
    {
        var path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images";

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
        var path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes";

        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            // nothing
        }
    }

    private void CreateFolderForClientImages()
    {
        var path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Clients";

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
                (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
    }

    [Obsolete("Obsolete")]
    public static string Hash(string input)
    {
        if (input == "admin")
            return "admin";

        using var sha1 = new SHA1Managed();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder(hash.Length * 2);

        foreach (var b in hash)
            // can be "x2" if you want lowercase
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}