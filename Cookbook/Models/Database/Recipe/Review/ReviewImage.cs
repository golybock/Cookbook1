namespace Cookbook.Models.Database.Recipe.Review;

public partial class ReviewImage
{
    public int Id { get; set; }

    public int ReviewId { get; set; }

    public string ImagePath { get; set; } = null!;
}