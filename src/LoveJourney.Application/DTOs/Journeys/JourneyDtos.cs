namespace LoveJourney.Application.DTOs.Journeys;

public class CreateJourneyRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string JourneyType { get; set; } = "other";
    public DateOnly JourneyDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

public class UpdateJourneyRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string JourneyType { get; set; } = "other";
    public DateOnly JourneyDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

public class JourneyResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string JourneyType { get; set; } = string.Empty;
    public DateOnly JourneyDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int PlaceCount { get; set; }
    public int PhotoCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class JourneyDetailResponse : JourneyResponse
{
    public List<PlaceInJourneyResponse> Places { get; set; } = new();
    public short? Rating { get; set; }
    public string? ReviewText { get; set; }
    public string? Highlights { get; set; }
    public bool? WouldRevisit { get; set; }
}

public class PlaceInJourneyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Category { get; set; }
    public short? Rating { get; set; }
}

public class TimelineGroupResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public List<JourneyResponse> Journeys { get; set; } = new();
}
