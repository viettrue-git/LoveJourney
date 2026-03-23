using LoveJourney.Application.Common.Interfaces;
using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Photos;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class PhotoService
{
    private readonly DbContext _db;
    private readonly IFileStorageService _fileStorage;

    public PhotoService(DbContext db, IFileStorageService fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<PagedResult<PhotoResponse>> GetPhotosAsync(
        Guid coupleId, Guid? journeyId, Guid? placeId, int page = 1, int size = 20)
    {
        var query = _db.Set<Photo>()
            .Where(p => p.CoupleId == coupleId)
            .AsQueryable();

        if (journeyId.HasValue)
            query = query.Where(p => p.JourneyId == journeyId.Value);
        if (placeId.HasValue)
            query = query.Where(p => p.PlaceId == placeId.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(p => new PhotoResponse
            {
                Id = p.Id,
                JourneyId = p.JourneyId,
                PlaceId = p.PlaceId,
                FileName = p.FileName,
                StorageUrl = p.StoragePath,
                ThumbnailUrl = p.ThumbnailPath,
                ContentType = p.ContentType,
                FileSizeBytes = p.FileSizeBytes,
                Caption = p.Caption,
                SortOrder = p.SortOrder,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        // Convert paths to URLs
        foreach (var item in items)
        {
            item.StorageUrl = _fileStorage.GetFullUrl(item.StorageUrl);
            if (item.ThumbnailUrl != null)
                item.ThumbnailUrl = _fileStorage.GetFullUrl(item.ThumbnailUrl);
        }

        return new PagedResult<PhotoResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = size
        };
    }

    public async Task<Result<PhotoResponse>> UploadAsync(
        Guid coupleId, Guid? journeyId, Guid? placeId,
        Stream fileStream, string fileName, string contentType, long fileSize)
    {
        var folder = "photos";
        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var storagePath = await _fileStorage.SaveAsync(fileStream, uniqueName, folder);

        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            JourneyId = journeyId,
            PlaceId = placeId,
            FileName = fileName,
            StoragePath = storagePath,
            ContentType = contentType,
            FileSizeBytes = fileSize
        };

        _db.Set<Photo>().Add(photo);
        await _db.SaveChangesAsync();

        return Result<PhotoResponse>.Ok(new PhotoResponse
        {
            Id = photo.Id,
            JourneyId = photo.JourneyId,
            PlaceId = photo.PlaceId,
            FileName = photo.FileName,
            StorageUrl = _fileStorage.GetFullUrl(photo.StoragePath),
            ThumbnailUrl = photo.ThumbnailPath != null ? _fileStorage.GetFullUrl(photo.ThumbnailPath) : null,
            ContentType = photo.ContentType,
            FileSizeBytes = photo.FileSizeBytes,
            Caption = photo.Caption,
            SortOrder = photo.SortOrder,
            CreatedAt = photo.CreatedAt
        });
    }

    public async Task<Result<PhotoResponse>> UpdateCaptionAsync(Guid coupleId, Guid photoId, string? caption)
    {
        var photo = await _db.Set<Photo>()
            .FirstOrDefaultAsync(p => p.Id == photoId && p.CoupleId == coupleId);

        if (photo == null)
            return Result<PhotoResponse>.Fail("Không tìm thấy ảnh.");

        photo.Caption = caption;
        await _db.SaveChangesAsync();

        return Result<PhotoResponse>.Ok(new PhotoResponse
        {
            Id = photo.Id,
            JourneyId = photo.JourneyId,
            PlaceId = photo.PlaceId,
            FileName = photo.FileName,
            StorageUrl = _fileStorage.GetFullUrl(photo.StoragePath),
            ThumbnailUrl = photo.ThumbnailPath != null ? _fileStorage.GetFullUrl(photo.ThumbnailPath) : null,
            ContentType = photo.ContentType,
            FileSizeBytes = photo.FileSizeBytes,
            Caption = photo.Caption,
            SortOrder = photo.SortOrder,
            CreatedAt = photo.CreatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid photoId)
    {
        var photo = await _db.Set<Photo>()
            .FirstOrDefaultAsync(p => p.Id == photoId && p.CoupleId == coupleId);

        if (photo == null)
            return Result<bool>.Fail("Không tìm thấy ảnh.");

        await _fileStorage.DeleteAsync(photo.StoragePath);
        if (photo.ThumbnailPath != null)
            await _fileStorage.DeleteAsync(photo.ThumbnailPath);

        _db.Set<Photo>().Remove(photo);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }
}
