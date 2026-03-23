using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Profile;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class ProfileService
{
    private readonly DbContext _db;

    public ProfileService(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<ProfileResponse>> GetProfileAsync(Guid coupleId)
    {
        var couple = await _db.Set<Couple>().FindAsync(coupleId);
        if (couple == null)
            return Result<ProfileResponse>.Fail("Không tìm thấy tài khoản.");

        return Result<ProfileResponse>.Ok(MapToResponse(couple));
    }

    public async Task<Result<ProfileResponse>> UpdateProfileAsync(Guid coupleId, UpdateProfileRequest request)
    {
        var couple = await _db.Set<Couple>().FindAsync(coupleId);
        if (couple == null)
            return Result<ProfileResponse>.Fail("Không tìm thấy tài khoản.");

        couple.Partner1Name = request.Partner1Name;
        couple.Partner2Name = request.Partner2Name;
        couple.StartDate = request.StartDate;
        couple.Timezone = request.Timezone;

        await _db.SaveChangesAsync();

        return Result<ProfileResponse>.Ok(MapToResponse(couple));
    }

    public async Task<Result<ProfileResponse>> UpdatePhotoUrlAsync(Guid coupleId, string photoUrl)
    {
        var couple = await _db.Set<Couple>().FindAsync(coupleId);
        if (couple == null)
            return Result<ProfileResponse>.Fail("Không tìm thấy tài khoản.");

        couple.ProfilePhotoUrl = photoUrl;
        await _db.SaveChangesAsync();

        return Result<ProfileResponse>.Ok(MapToResponse(couple));
    }

    private static ProfileResponse MapToResponse(Couple couple)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var start = couple.StartDate;
        var totalDays = today.DayNumber - start.DayNumber;

        // Calculate years, months, days
        var years = 0;
        var months = 0;
        var days = 0;

        var temp = start;
        while (temp.AddYears(years + 1) <= today) years++;
        temp = start.AddYears(years);
        while (temp.AddMonths(months + 1) <= today) months++;
        temp = temp.AddMonths(months);
        days = today.DayNumber - temp.DayNumber;

        return new ProfileResponse
        {
            Id = couple.Id,
            Email = couple.Email,
            Partner1Name = couple.Partner1Name,
            Partner2Name = couple.Partner2Name,
            StartDate = couple.StartDate,
            ProfilePhotoUrl = couple.ProfilePhotoUrl,
            Timezone = couple.Timezone,
            Duration = new DurationInfo
            {
                Years = years,
                Months = months,
                Days = days,
                TotalDays = totalDays
            }
        };
    }
}
