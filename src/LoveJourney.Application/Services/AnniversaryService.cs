using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Anniversaries;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class AnniversaryService
{
    private readonly DbContext _db;

    public AnniversaryService(DbContext db)
    {
        _db = db;
    }

    public async Task<List<AnniversaryResponse>> GetAllAsync(Guid coupleId)
    {
        var anniversaries = await _db.Set<Anniversary>()
            .Where(a => a.CoupleId == coupleId)
            .OrderBy(a => a.Date)
            .ToListAsync();

        return anniversaries.Select(MapToResponse).ToList();
    }

    public async Task<List<AnniversaryResponse>> GetUpcomingAsync(Guid coupleId, int days = 30)
    {
        var anniversaries = await _db.Set<Anniversary>()
            .Where(a => a.CoupleId == coupleId && a.IsActive)
            .ToListAsync();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var cutoff = today.AddDays(days);

        return anniversaries
            .Select(MapToResponse)
            .Where(a => a.NextOccurrence.HasValue
                        && a.NextOccurrence.Value >= today
                        && a.NextOccurrence.Value <= cutoff)
            .OrderBy(a => a.NextOccurrence)
            .ToList();
    }

    public async Task<Result<AnniversaryResponse>> CreateAsync(Guid coupleId, CreateAnniversaryRequest request)
    {
        var anniversary = new Anniversary
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            Title = request.Title,
            Date = request.Date,
            Recurrence = request.Recurrence,
            ReminderDaysBefore = request.ReminderDaysBefore,
            Notes = request.Notes
        };

        _db.Set<Anniversary>().Add(anniversary);
        await _db.SaveChangesAsync();

        return Result<AnniversaryResponse>.Ok(MapToResponse(anniversary));
    }

    public async Task<Result<AnniversaryResponse>> UpdateAsync(Guid coupleId, Guid id, UpdateAnniversaryRequest request)
    {
        var anniversary = await _db.Set<Anniversary>()
            .FirstOrDefaultAsync(a => a.Id == id && a.CoupleId == coupleId);

        if (anniversary == null)
            return Result<AnniversaryResponse>.Fail("Không tìm thấy ngày kỷ niệm.");

        anniversary.Title = request.Title;
        anniversary.Date = request.Date;
        anniversary.Recurrence = request.Recurrence;
        anniversary.ReminderDaysBefore = request.ReminderDaysBefore;
        anniversary.Notes = request.Notes;
        anniversary.IsActive = request.IsActive;

        await _db.SaveChangesAsync();

        return Result<AnniversaryResponse>.Ok(MapToResponse(anniversary));
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid id)
    {
        var anniversary = await _db.Set<Anniversary>()
            .FirstOrDefaultAsync(a => a.Id == id && a.CoupleId == coupleId);

        if (anniversary == null)
            return Result<bool>.Fail("Không tìm thấy ngày kỷ niệm.");

        _db.Set<Anniversary>().Remove(anniversary);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }

    private static AnniversaryResponse MapToResponse(Anniversary a)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var nextOccurrence = CalculateNextOccurrence(a.Date, a.Recurrence, today);
        int? daysUntil = nextOccurrence.HasValue
            ? nextOccurrence.Value.DayNumber - today.DayNumber
            : null;

        return new AnniversaryResponse
        {
            Id = a.Id,
            Title = a.Title,
            Date = a.Date,
            Recurrence = a.Recurrence,
            ReminderDaysBefore = a.ReminderDaysBefore,
            Notes = a.Notes,
            IsActive = a.IsActive,
            NextOccurrence = nextOccurrence,
            DaysUntilNext = daysUntil,
            CreatedAt = a.CreatedAt
        };
    }

    private static DateOnly? CalculateNextOccurrence(DateOnly originalDate, string recurrence, DateOnly today)
    {
        if (recurrence == "none")
            return originalDate >= today ? originalDate : null;

        if (recurrence == "yearly")
        {
            var thisYear = new DateOnly(today.Year, originalDate.Month, originalDate.Day);
            return thisYear >= today ? thisYear : thisYear.AddYears(1);
        }

        if (recurrence == "monthly")
        {
            var day = Math.Min(originalDate.Day, DateTime.DaysInMonth(today.Year, today.Month));
            var thisMonth = new DateOnly(today.Year, today.Month, day);
            if (thisMonth >= today)
                return thisMonth;

            var nextMonth = today.AddMonths(1);
            day = Math.Min(originalDate.Day, DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month));
            return new DateOnly(nextMonth.Year, nextMonth.Month, day);
        }

        return null;
    }
}
