namespace LoveJourney.Application.DTOs.Photos;

public class PhotoResponse
{
    public Guid Id { get; set; }
    public Guid? JourneyId { get; set; }
    public Guid? PlaceId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StorageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string? Caption { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdatePhotoCaptionRequest
{
    public string? Caption { get; set; }
}
