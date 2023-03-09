using System;
using Models.Models.Database.Recipe;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Cookbook.FIleGenerating;

public class RecipePdf
{
    public static string Generate(Recipe recipe)
    {
        PdfDocument doc = new PdfDocument();
        doc.Info.Title = recipe.Name;

        PdfPage page = doc.AddPage();

        XGraphics xGraphics = XGraphics.FromPdfPage(page);

        XFont font = new XFont("Times new roman", 20, XFontStyle.Bold);
        
        xGraphics.DrawString(recipe.Name, font, XBrushes.Black, new XRect(0,0, page.Width, page.Height), XStringFormats.Center);

        string path = $"C:\\Users\\{Environment.UserName}\\Documents\\Images\\Recipes\\{recipe.Name}.pdf";
        
        doc.Save(path);

        return path;
    }
}