using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.BlogPosts;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class BlogPostService
{
    private readonly DbContext _db;

    public BlogPostService(DbContext db)
    {
        _db = db;
    }

    // Public: get published posts (no auth required)
    public async Task<PagedResult<BlogPostSummaryResponse>> GetPublishedAsync(int page = 1, int size = 10)
    {
        var query = _db.Set<BlogPost>()
            .Where(b => b.Status == "published")
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(b => b.PublishedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(b => new BlogPostSummaryResponse
            {
                Id = b.Id,
                Title = b.Title,
                Excerpt = b.Content.Length > 200 ? b.Content.Substring(0, 200) + "..." : b.Content,
                CoverImageUrl = b.CoverImageUrl,
                AuthorNames = b.Couple.Partner1Name + " & " + b.Couple.Partner2Name,
                PublishedAt = b.PublishedAt
            })
            .ToListAsync();

        return new PagedResult<BlogPostSummaryResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = size
        };
    }

    // Public: get single published post
    public async Task<Result<BlogPostResponse>> GetPublishedByIdAsync(Guid id)
    {
        var post = await _db.Set<BlogPost>()
            .Where(b => b.Id == id && b.Status == "published")
            .Select(b => new BlogPostResponse
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CoverImageUrl = b.CoverImageUrl,
                Status = b.Status,
                AuthorNames = b.Couple.Partner1Name + " & " + b.Couple.Partner2Name,
                PublishedAt = b.PublishedAt,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (post == null)
            return Result<BlogPostResponse>.Fail("Không tìm thấy bài viết.");

        return Result<BlogPostResponse>.Ok(post);
    }

    // Auth: get own posts (all statuses)
    public async Task<PagedResult<BlogPostResponse>> GetMyPostsAsync(Guid coupleId, int page = 1, int size = 10)
    {
        var query = _db.Set<BlogPost>()
            .Where(b => b.CoupleId == coupleId);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(b => new BlogPostResponse
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CoverImageUrl = b.CoverImageUrl,
                Status = b.Status,
                AuthorNames = b.Couple.Partner1Name + " & " + b.Couple.Partner2Name,
                PublishedAt = b.PublishedAt,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<BlogPostResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = size
        };
    }

    // Auth: get own post by id (any status)
    public async Task<Result<BlogPostResponse>> GetMyPostByIdAsync(Guid coupleId, Guid id)
    {
        var post = await _db.Set<BlogPost>()
            .Where(b => b.Id == id && b.CoupleId == coupleId)
            .Select(b => new BlogPostResponse
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CoverImageUrl = b.CoverImageUrl,
                Status = b.Status,
                AuthorNames = b.Couple.Partner1Name + " & " + b.Couple.Partner2Name,
                PublishedAt = b.PublishedAt,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (post == null)
            return Result<BlogPostResponse>.Fail("Không tìm thấy bài viết.");

        return Result<BlogPostResponse>.Ok(post);
    }

    public async Task<Result<BlogPostResponse>> CreateAsync(Guid coupleId, CreateBlogPostRequest request)
    {
        var post = new BlogPost
        {
            Id = Guid.NewGuid(),
            CoupleId = coupleId,
            Title = request.Title,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            Status = request.Status,
            PublishedAt = request.Status == "published" ? DateTime.UtcNow : null
        };

        _db.Set<BlogPost>().Add(post);
        await _db.SaveChangesAsync();

        var couple = await _db.Set<Couple>().FindAsync(coupleId);

        return Result<BlogPostResponse>.Ok(new BlogPostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CoverImageUrl = post.CoverImageUrl,
            Status = post.Status,
            AuthorNames = couple != null ? $"{couple.Partner1Name} & {couple.Partner2Name}" : "",
            PublishedAt = post.PublishedAt,
            CreatedAt = post.CreatedAt
        });
    }

    public async Task<Result<BlogPostResponse>> UpdateAsync(Guid coupleId, Guid id, UpdateBlogPostRequest request)
    {
        var post = await _db.Set<BlogPost>()
            .Include(b => b.Couple)
            .FirstOrDefaultAsync(b => b.Id == id && b.CoupleId == coupleId);

        if (post == null)
            return Result<BlogPostResponse>.Fail("Không tìm thấy bài viết.");

        // Set PublishedAt when first published
        if (post.Status != "published" && request.Status == "published")
            post.PublishedAt = DateTime.UtcNow;

        post.Title = request.Title;
        post.Content = request.Content;
        post.CoverImageUrl = request.CoverImageUrl;
        post.Status = request.Status;

        await _db.SaveChangesAsync();

        return Result<BlogPostResponse>.Ok(new BlogPostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CoverImageUrl = post.CoverImageUrl,
            Status = post.Status,
            AuthorNames = $"{post.Couple.Partner1Name} & {post.Couple.Partner2Name}",
            PublishedAt = post.PublishedAt,
            CreatedAt = post.CreatedAt
        });
    }

    public async Task<Result<bool>> DeleteAsync(Guid coupleId, Guid id)
    {
        var post = await _db.Set<BlogPost>()
            .FirstOrDefaultAsync(b => b.Id == id && b.CoupleId == coupleId);

        if (post == null)
            return Result<bool>.Fail("Không tìm thấy bài viết.");

        _db.Set<BlogPost>().Remove(post);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }
}
