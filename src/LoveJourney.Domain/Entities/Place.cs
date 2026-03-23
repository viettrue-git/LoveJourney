namespace LoveJourney.Domain.Entities;

public class Place
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public Guid CoupleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
    public string? Category { get; set; } // restaurant, hotel, attraction, cafe, other
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Journey Journey { get; set; } = null!;
    public Couple Couple { get; set; } = null!;
    public PlaceReview? Review { get; set; }
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
