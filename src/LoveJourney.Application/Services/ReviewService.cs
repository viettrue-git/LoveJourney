using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Reviews;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class ReviewService
{
    private readonly DbContext _db;

    public ReviewService(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<ReviewResponse>> GetByPlaceAsync(Guid coupleId, Guid placeId)
    {
        var review = await _db.Set<PlaceReview>()
            .Where(r => r.PlaceId == placeId && r.CoupleId == coupleId)
            .Select(r => new ReviewResponse
            {
                Id = r.Id,
                PlaceId = r.PlaceId,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                Tips = r.Tips,
                WouldRevisit = r.WouldRevisit,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (review == null)
            return Result<ReviewResponse>.Fail("Chưa có đánh giá.");

        return Result<ReviewResponse>.Ok(review);
    }

    public async Task<Result<ReviewResponse>> CreateOrUpdateAsync(Guid coupleId, Guid placeId, CreateReviewRequest request)
    {
        var placeExists = await _db.Set<Place>()
            .AnyAsync(p => p.Id == placeId && p.CoupleId == coupleId);

        if (!placeExists)
            return Result<ReviewResponse>.Fail("Không tìm thấy địa điểm.");

        var existing = await _db.Set<PlaceReview>()
            .FirstOrDefaultAsync(r => r.PlaceId == placeId && r.CoupleId == coupleId);

        if (existing != null)
        {
            existing.Rating = request.Rating;
            existing.ReviewText = request.ReviewText;
            existing.Tips = request.Tips;
            existing.WouldRevisit = request.WouldRevisit;
        }
        else
        {
            existing = new PlaceReview
            {
                Id = Guid.NewGuid(),
                PlaceId = placeId,
                CoupleId = coupleId,
                Rating = request.Rating,
                ReviewText = request.ReviewText,
                Tips = request.Tips,
                WouldRevisit = request.WouldRevisit
            };
            _db.Set<PlaceReview>().Add(existing);
        }

        await _db.SaveChangesAsync();

        return Result<ReviewResponse>.Ok(new ReviewResponse
        {
            Id = existing.Id,
            PlaceId = existing.PlaceId,
            Rating = existing.Rating,
            ReviewText = existing.ReviewText,
            Tips = existing.Tips,
            WouldRevisit = existing.WouldRevisit,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = existing.UpdatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid placeId)
    {
        var review = await _db.Set<PlaceReview>()
            .FirstOrDefaultAsync(r => r.PlaceId == placeId && r.CoupleId == coupleId);

        if (review == null)
            return Result<bool>.Fail("Không tìm thấy đánh giá.");

        _db.Set<PlaceReview>().Remove(review);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }
}
