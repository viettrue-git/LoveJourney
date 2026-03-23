namespace LoveJourney.Application.DTOs.JourneyReviews;

public class CreateJourneyReviewRequest
{
    public short Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Highlights { get; set; }
    public bool WouldRevisit { get; set; }
}

public class JourneyReviewResponse
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public short Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Highlights { get; set; }
    public bool WouldRevisit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
