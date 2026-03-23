using LoveJourney.Application.DTOs.Anniversaries;
using LoveJourney.Application.DTOs.Journeys;
using LoveJourney.Application.DTOs.Profile;

namespace LoveJourney.Application.DTOs.Dashboard;

public class DashboardResponse
{
    public DurationInfo Duration { get; set; } = new();
    public int TotalJourneys { get; set; }
    public int TotalPlaces { get; set; }
    public int TotalPhotos { get; set; }
    public List<AnniversaryResponse> UpcomingAnniversaries { get; set; } = new();
    public List<JourneyResponse> RecentJourneys { get; set; } = new();
}
