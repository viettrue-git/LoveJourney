namespace LoveJourney.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public Guid? JourneyId { get; set; }
    public Guid? PlaceId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string? ThumbnailPath { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string? Caption { get; set; }
    public DateTime? TakenAt { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Couple Couple { get; set; } = null!;
    public Journey? Journey { get; set; }
    public Place? Place { get; set; }
}
