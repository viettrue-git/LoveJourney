namespace LoveJourney.Domain.Entities;

public class JourneyReview
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public Guid CoupleId { get; set; }
    public short Rating { get; set; } // 1-5
    public string? ReviewText { get; set; }
    public string? Highlights { get; set; }
    public bool WouldRevisit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Journey Journey { get; set; } = null!;
    public Couple Couple { get; set; } = null!;
}
