using LoveJourney.Application.DTOs.Dashboard;
using LoveJourney.Application.DTOs.Journeys;
using LoveJourney.Application.DTOs.Profile;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class DashboardService
{
    private readonly DbContext _db;
    private readonly AnniversaryService _anniversaryService;

    public DashboardService(DbContext db, AnniversaryService anniversaryService)
    {
        _db = db;
        _anniversaryService = anniversaryService;
    }

    public async Task<DashboardResponse> GetDashboardAsync(Guid coupleId)
    {
        var couple = await _db.Set<Couple>().FindAsync(coupleId);
        if (couple == null) return new DashboardResponse();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var start = couple.StartDate;
        var totalDays = today.DayNumber - start.DayNumber;

        var years = 0; var months = 0; var days = 0;
        var temp = start;
        while (temp.AddYears(years + 1) <= today) years++;
        temp = start.AddYears(years);
        while (temp.AddMonths(months + 1) <= today) months++;
        temp = temp.AddMonths(months);
        days = today.DayNumber - temp.DayNumber;

        var totalJourneys = await _db.Set<Journey>().CountAsync(j => j.CoupleId == coupleId);
        var totalPlaces = await _db.Set<Place>().CountAsync(p => p.CoupleId == coupleId);
        var totalPhotos = await _db.Set<Photo>().CountAsync(p => p.CoupleId == coupleId);

        var upcoming = await _anniversaryService.GetUpcomingAsync(coupleId, 30);

        var recentJourneys = await _db.Set<Journey>()
            .Where(j => j.CoupleId == coupleId)
            .OrderByDescending(j => j.JourneyDate)
            .Take(5)
            .Select(j => new JourneyResponse
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                JourneyType = j.JourneyType,
                JourneyDate = j.JourneyDate,
                EndDate = j.EndDate,
                PlaceCount = j.Places.Count,
                PhotoCount = j.Photos.Count,
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();

        return new DashboardResponse
        {
            Duration = new DurationInfo
            {
                Years = years,
                Months = months,
                Days = days,
                TotalDays = totalDays
            },
            TotalJourneys = totalJourneys,
            TotalPlaces = totalPlaces,
            TotalPhotos = totalPhotos,
            UpcomingAnniversaries = upcoming,
            RecentJourneys = recentJourneys
        };
    }
}
