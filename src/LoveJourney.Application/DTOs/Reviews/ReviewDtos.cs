namespace LoveJourney.Application.DTOs.Reviews;

public class CreateReviewRequest
{
    public short Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Tips { get; set; }
    public bool WouldRevisit { get; set; }
}

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public short Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Tips { get; set; }
    public bool WouldRevisit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
