namespace LoveJourney.Domain.Entities;

public class BlogPost
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Status { get; set; } = "draft"; // draft, published
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Couple Couple { get; set; } = null!;
}
