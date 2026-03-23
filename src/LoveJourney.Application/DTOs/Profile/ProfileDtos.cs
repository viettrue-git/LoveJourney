namespace LoveJourney.Application.DTOs.Profile;

public class ProfileResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Partner1Name { get; set; } = string.Empty;
    public string Partner2Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public DurationInfo Duration { get; set; } = new();
}

public class DurationInfo
{
    public int Years { get; set; }
    public int Months { get; set; }
    public int Days { get; set; }
    public int TotalDays { get; set; }
}

public class UpdateProfileRequest
{
    public string Partner1Name { get; set; } = string.Empty;
    public string Partner2Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public string Timezone { get; set; } = "UTC";
}
