namespace LoveJourney.Application.DTOs.BlogPosts;

public class CreateBlogPostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Status { get; set; } = "draft"; // draft, published
}

public class UpdateBlogPostRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Status { get; set; } = "draft";
}

public class BlogPostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public string AuthorNames { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BlogPostSummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string AuthorNames { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
}
