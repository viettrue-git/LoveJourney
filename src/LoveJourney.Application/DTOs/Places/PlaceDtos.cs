namespace LoveJourney.Application.DTOs.Places;

public class CreatePlaceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
    public string? Category { get; set; }
}

public class UpdatePlaceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
    public string? Category { get; set; }
}

public class PlaceResponse
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
    public string? Category { get; set; }
    public short? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MapPlaceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Category { get; set; }
    public string? Address { get; set; }
    public short? Rating { get; set; }
    public string JourneyTitle { get; set; } = string.Empty;
    public Guid JourneyId { get; set; }
}
