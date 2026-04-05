using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.JourneyReviews;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class JourneyReviewService
{
    private readonly DbContext _db;

    public JourneyReviewService(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<JourneyReviewResponse>> GetByJourneyAsync(Guid coupleId, Guid journeyId)
    {
        var review = await _db.Set<JourneyReview>()
            .Where(r => r.JourneyId == journeyId && r.CoupleId == coupleId)
            .Select(r => new JourneyReviewResponse
            {
                Id = r.Id,
                JourneyId = r.JourneyId,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                Highlights = r.Highlights,
                WouldRevisit = r.WouldRevisit,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (review == null)
            return Result<JourneyReviewResponse>.Fail("Chưa có đánh giá.");

        return Result<JourneyReviewResponse>.Ok(review);
    }

    public async Task<Result<JourneyReviewResponse>> CreateOrUpdateAsync(Guid coupleId, Guid journeyId, CreateJourneyReviewRequest request)
    {
        var journeyExists = await _db.Set<Journey>()
            .AnyAsync(j => j.Id == journeyId && j.CoupleId == coupleId);

        if (!journeyExists)
            return Result<JourneyReviewResponse>.Fail("Không tìm thấy hành trình.");

        var existing = await _db.Set<JourneyReview>()
            .FirstOrDefaultAsync(r => r.JourneyId == journeyId && r.CoupleId == coupleId);

        if (existing != null)
        {
            existing.Rating = request.Rating;
            existing.ReviewText = request.ReviewText;
            existing.Highlights = request.Highlights;
            existing.WouldRevisit = request.WouldRevisit;
        }
        else
        {
            existing = new JourneyReview
            {
                Id = Guid.NewGuid(),
                JourneyId = journeyId,
                CoupleId = coupleId,
                Rating = request.Rating,
                ReviewText = request.ReviewText,
                Highlights = request.Highlights,
                WouldRevisit = request.WouldRevisit
            };
            _db.Set<JourneyReview>().Add(existing);
        }

        await _db.SaveChangesAsync();

        return Result<JourneyReviewResponse>.Ok(new JourneyReviewResponse
        {
            Id = existing.Id,
            JourneyId = existing.JourneyId,
            Rating = existing.Rating,
            ReviewText = existing.ReviewText,
            Highlights = existing.Highlights,
            WouldRevisit = existing.WouldRevisit,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = existing.UpdatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid journeyId)
    {
        var review = await _db.Set<JourneyReview>()
            .FirstOrDefaultAsync(r => r.JourneyId == journeyId && r.CoupleId == coupleId);

        if (review == null)
            return Result<bool>.Fail("Không tìm thấy đánh giá.");

        _db.Set<JourneyReview>().Remove(review);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }
}
