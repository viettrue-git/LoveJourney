namespace LoveJourney.Domain.Entities;

public class PlaceReview
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public Guid CoupleId { get; set; }
    public short Rating { get; set; } // 1-5
    public string? ReviewText { get; set; }
    public string? Tips { get; set; }
    public bool WouldRevisit { get; set; }
    public DateOnly? VisitedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Place Place { get; set; } = null!;
    public Couple Couple { get; set; } = null!;
}
