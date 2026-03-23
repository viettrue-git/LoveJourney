using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Journeys;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class JourneyService
{
    private readonly DbContext _db;

    public JourneyService(DbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<JourneyResponse>> GetJourneysAsync(
        Guid coupleId, string? type, DateOnly? from, DateOnly? to, int page = 1, int size = 10)
    {
        var query = _db.Set<Journey>()
            .Where(j => j.CoupleId == coupleId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(type))
            query = query.Where(j => j.JourneyType == type);
        if (from.HasValue)
            query = query.Where(j => j.JourneyDate >= from.Value);
        if (to.HasValue)
            query = query.Where(j => j.JourneyDate <= to.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(j => j.JourneyDate)
            .Skip((page - 1) * size)
            .Take(size)
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

        return new PagedResult<JourneyResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = size
        };
    }

    public async Task<Result<JourneyDetailResponse>> GetJourneyByIdAsync(Guid coupleId, Guid journeyId)
    {
        var journey = await _db.Set<Journey>()
            .Where(j => j.Id == journeyId && j.CoupleId == coupleId)
            .Select(j => new JourneyDetailResponse
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                JourneyType = j.JourneyType,
                JourneyDate = j.JourneyDate,
                EndDate = j.EndDate,
                PlaceCount = j.Places.Count,
                PhotoCount = j.Photos.Count,
                CreatedAt = j.CreatedAt,
                Places = j.Places.Select(p => new PlaceInJourneyResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Address = p.Address,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Category = p.Category,
                    Rating = p.Review != null ? p.Review.Rating : null
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (journey == null)
            return Result<JourneyDetailResponse>.Fail("Không tìm thấy hành trình.");

        return Result<JourneyDetailResponse>.Ok(journey);
    }

    public async Task<List<TimelineGroupResponse>> GetTimelineAsync(Guid coupleId)
    {
        var journeys = await _db.Set<Journey>()
            .Where(j => j.CoupleId == coupleId)
            .OrderByDescending(j => j.JourneyDate)
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

        return journeys
            .GroupBy(j => new { j.JourneyDate.Year, j.JourneyDate.Month })
            .Select(g => new TimelineGroupResponse
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Journeys = g.ToList()
            })
            .ToList();
    }

    public async Task<Result<JourneyResponse>> CreateAsync(Guid coupleId, CreateJourneyRequest request)
    {
        var journey = new Journey
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            Title = request.Title,
            Description = request.Description,
            JourneyType = request.JourneyType,
            JourneyDate = request.JourneyDate,
            EndDate = request.EndDate
        };

        _db.Set<Journey>().Add(journey);
        await _db.SaveChangesAsync();

        return Result<JourneyResponse>.Ok(new JourneyResponse
        {
            Id = journey.Id,
            Title = journey.Title,
            Description = journey.Description,
            JourneyType = journey.JourneyType,
            JourneyDate = journey.JourneyDate,
            EndDate = journey.EndDate,
            PlaceCount = 0,
            PhotoCount = 0,
            CreatedAt = journey.CreatedAt
        });
    }

    public async Task<Result<JourneyResponse>> UpdateAsync(Guid coupleId, Guid journeyId, UpdateJourneyRequest request)
    {
        var journey = await _db.Set<Journey>()
            .Include(j => j.Places)
            .Include(j => j.Photos)
            .FirstOrDefaultAsync(j => j.Id == journeyId && j.CoupleId == coupleId);

        if (journey == null)
            return Result<JourneyResponse>.Fail("Không tìm thấy hành trình.");

        journey.Title = request.Title;
        journey.Description = request.Description;
        journey.JourneyType = request.JourneyType;
        journey.JourneyDate = request.JourneyDate;
        journey.EndDate = request.EndDate;

        await _db.SaveChangesAsync();

        return Result<JourneyResponse>.Ok(new JourneyResponse
        {
            Id = journey.Id,
            Title = journey.Title,
            Description = journey.Description,
            JourneyType = journey.JourneyType,
            JourneyDate = journey.JourneyDate,
            EndDate = journey.EndDate,
            PlaceCount = journey.Places.Count,
            PhotoCount = journey.Photos.Count,
            CreatedAt = journey.CreatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid journeyId)
    {
        var journey = await _db.Set<Journey>()
            .FirstOrDefaultAsync(j => j.Id == journeyId && j.CoupleId == coupleId);

        if (journey == null)
            return Result<bool>.Fail("Không tìm thấy hành trình.");

        _db.Set<Journey>().Remove(journey);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }
}
