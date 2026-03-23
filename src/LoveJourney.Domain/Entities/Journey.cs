namespace LoveJourney.Domain.Entities;

public class Journey
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string JourneyType { get; set; } = "other"; // trip, dining, activity, other
    public DateOnly JourneyDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Couple Couple { get; set; } = null!;
    public ICollection<Place> Places { get; set; } = new List<Place>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public JourneyReview? Review { get; set; }
}
