using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Places;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class PlaceService
{
    private readonly DbContext _db;

    public PlaceService(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<List<PlaceResponse>>> GetByJourneyAsync(Guid coupleId, Guid journeyId)
    {
        var journeyExists = await _db.Set<Journey>()
            .AnyAsync(j => j.Id == journeyId && j.CoupleId == coupleId);

        if (!journeyExists)
            return Result<List<PlaceResponse>>.Fail("Không tìm thấy hành trình.");

        var places = await _db.Set<Place>()
            .Where(p => p.JourneyId == journeyId && p.CoupleId == coupleId)
            .Select(p => new PlaceResponse
            {
                Id = p.Id,
                JourneyId = p.JourneyId,
                Name = p.Name,
                Address = p.Address,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                GooglePlaceId = p.GooglePlaceId,
                Category = p.Category,
                Rating = p.Review != null ? p.Review.Rating : null,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Result<List<PlaceResponse>>.Ok(places);
    }

    public async Task<Result<PlaceResponse>> CreateAsync(Guid coupleId, Guid journeyId, CreatePlaceRequest request)
    {
        var journeyExists = await _db.Set<Journey>()
            .AnyAsync(j => j.Id == journeyId && j.CoupleId == coupleId);

        if (!journeyExists)
            return Result<PlaceResponse>.Fail("Không tìm thấy hành trình.");

        var place = new Place
        {
            Id = Guid.NewGuid(),
            JourneyId = journeyId,
            CoupleId = coupleId,
            Name = request.Name,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            GooglePlaceId = request.GooglePlaceId,
            Category = request.Category
        };

        _db.Set<Place>().Add(place);
        await _db.SaveChangesAsync();

        return Result<PlaceResponse>.Ok(new PlaceResponse
        {
            Id = place.Id,
            JourneyId = place.JourneyId,
            Name = place.Name,
            Address = place.Address,
            Latitude = place.Latitude,
            Longitude = place.Longitude,
            GooglePlaceId = place.GooglePlaceId,
            Category = place.Category,
            CreatedAt = place.CreatedAt
        });
    }

    public async Task<Result<PlaceResponse>> UpdateAsync(Guid coupleId, Guid placeId, UpdatePlaceRequest request)
    {
        var place = await _db.Set<Place>()
            .FirstOrDefaultAsync(p => p.Id == placeId && p.CoupleId == coupleId);

        if (place == null)
            return Result<PlaceResponse>.Fail("Không tìm thấy địa điểm.");

        place.Name = request.Name;
        place.Address = request.Address;
        place.Latitude = request.Latitude;
        place.Longitude = request.Longitude;
        place.GooglePlaceId = request.GooglePlaceId;
        place.Category = request.Category;

        await _db.SaveChangesAsync();

        return Result<PlaceResponse>.Ok(new PlaceResponse
        {
            Id = place.Id,
            JourneyId = place.JourneyId,
            Name = place.Name,
            Address = place.Address,
            Latitude = place.Latitude,
            Longitude = place.Longitude,
            GooglePlaceId = place.GooglePlaceId,
            Category = place.Category,
            CreatedAt = place.CreatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid placeId)
    {
        var place = await _db.Set<Place>()
            .FirstOrDefaultAsync(p => p.Id == placeId && p.CoupleId == coupleId);

        if (place == null)
            return Result<bool>.Fail("Không tìm thấy địa điểm.");

        _db.Set<Place>().Remove(place);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }

    public async Task<List<MapPlaceResponse>> GetMapPlacesAsync(Guid coupleId)
    {
        return await _db.Set<Place>()
            .Where(p => p.CoupleId == coupleId && p.Latitude != null && p.Longitude != null)
            .Select(p => new MapPlaceResponse
            {
                Id = p.Id,
                Name = p.Name,
                Latitude = p.Latitude!.Value,
                Longitude = p.Longitude!.Value,
                Category = p.Category,
                Address = p.Address,
                Rating = p.Review != null ? p.Review.Rating : null,
                JourneyTitle = p.Journey.Title,
                JourneyId = p.JourneyId
            })
            .ToListAsync();
    }
}
