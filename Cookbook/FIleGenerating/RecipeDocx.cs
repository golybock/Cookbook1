using System;
using System.Diagnostics;
using Models.Models.Database.Recipe;
using ModernWpf.Controls;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Cookbook.FIleGenerating;

public class RecipeDocx
{
    public static void Generate(Recipe recipe)
    {
        string path = $"C:\\Users\\{Environment.UserName}\\Documents\\";

        string fileName = recipe.Name + ".docx";

        string fullPath = path + fileName;

        try
        {
            using var doc = DocX.Create(fullPath);
            
            doc.InsertParagraph(recipe.Name).FontSize(20d).SpacingAfter(20d).Bold().Alignment = Alignment.left;

            try
            {
                var image = doc.AddImage(recipe.ImagePath);
                var pic = image.CreatePicture(250f, 250f);
                
                var p = doc.InsertParagraph("");
                p.AppendPicture(pic);
            }
            catch (Exception e)
            {
                // ignored
            }

            doc.InsertParagraph(GenerateRecipeString(recipe));

            doc.Save();
            
            ShowDialog(fullPath);
        }
        catch (Exception e)
        {
            ShowErrorDialog("Ошибка генерации файла");
        }
        
    }

    private static string GenerateRecipeString(Recipe recipe)
    {
        string category = recipe.Category.Name == String.Empty ?
            "Нет катeгории" :
            recipe.Category.Name;
        
        string categoryString = $"Категория: {category}";

        string recipeType = recipe.RecipeType.Name == string.Empty ?
            "Нет типа" :
            recipe.RecipeType.Name;

        string recipeTypeString = $"Тип: {recipeType}";

        string recipeText = string.IsNullOrWhiteSpace(recipe.Text) ?
            "Нет шагов приготовлния" :
            recipe.Text;

        string text = $"Шаги приготовления: \n \t{recipeText}";

        return $"{categoryString}\n{recipeTypeString}\n{text}";
    }
    
    private static async void ShowErrorDialog(string error)
    {
        ContentDialog addDialog = new ContentDialog()
        {
            Title = "Ошибка",
            Content = error,
            CloseButtonText = "Закрыть",
        };
        
        await addDialog.ShowAsync();
    }

    
    private static async void ShowDialog(string path)
    {
        ContentDialog acceptDialog = new ContentDialog()
        {
            Title = "Файл готов",
            Content = "Открыть файл?",
            CloseButtonText = "Не открывать",
            PrimaryButtonText = "Открыть",
            DefaultButton = ContentDialogButton.Primary
        };
        
        ContentDialogResult result = await acceptDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
            ShowFileInExplorer(path);
    }
    
    private static void ShowFileInExplorer(string path)
    {
        Process.Start(new ProcessStartInfo
            {
                FileName = "explorer", Arguments = $"/n, /select, {path}"
            }
        );
    }
}