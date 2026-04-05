namespace LoveJourney.Domain.Entities;

public class Couple
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Partner1Name { get; set; } = string.Empty;
    public string Partner2Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string Timezone { get; set; } = "UTC";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Anniversary> Anniversaries { get; set; } = new List<Anniversary>();
    public ICollection<Journey> Journeys { get; set; } = new List<Journey>();
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
