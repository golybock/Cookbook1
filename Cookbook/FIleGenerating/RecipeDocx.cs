using System;
using Models.Models.Database.Recipe;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Cookbook.FIleGenerating;

public class RecipeDocx
{
    public static void Generate(Recipe recipe)
    {
        string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\";
        
        using (var doc = DocX.Create(path + recipe.Name + ".docx"))
        {
            doc.InsertParagraph(recipe.Name).FontSize(20d).SpacingAfter(20d).Bold().Alignment = Alignment.left;

            var image = doc.AddImage(recipe.ImagePath);
            var pic = image.CreatePicture(250f, 250f);

            var p = doc.InsertParagraph("");
            p.AppendPicture(pic);
            
            doc.Save();
        }
    }

    private static async void ShowDialog(string path)
    {
        
    }
}