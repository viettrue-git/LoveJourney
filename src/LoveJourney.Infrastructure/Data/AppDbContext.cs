using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Couple> Couples => Set<Couple>();
    public DbSet<Anniversary> Anniversaries => Set<Anniversary>();
    public DbSet<Journey> Journeys => Set<Journey>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<PlaceReview> PlaceReviews => Set<PlaceReview>();
    public DbSet<JourneyReview> JourneyReviews => Set<JourneyReview>();
    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is Couple c) { c.CreatedAt = now; c.UpdatedAt = now; }
                else if (entry.Entity is Journey j) { j.CreatedAt = now; j.UpdatedAt = now; }
                else if (entry.Entity is PlaceReview r) { r.CreatedAt = now; r.UpdatedAt = now; }
                else if (entry.Entity is JourneyReview jr) { jr.CreatedAt = now; jr.UpdatedAt = now; }
                else if (entry.Entity is BlogPost bp) { bp.CreatedAt = now; bp.UpdatedAt = now; }
                else if (entry.Entity is Anniversary a) a.CreatedAt = now;
                else if (entry.Entity is Place p) p.CreatedAt = now;
                else if (entry.Entity is Photo ph) ph.CreatedAt = now;
                else if (entry.Entity is RefreshToken rt) rt.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is Couple c) c.UpdatedAt = now;
                else if (entry.Entity is Journey j) j.UpdatedAt = now;
                else if (entry.Entity is PlaceReview r) r.UpdatedAt = now;
                else if (entry.Entity is JourneyReview jr) jr.UpdatedAt = now;
                else if (entry.Entity is BlogPost bp) bp.UpdatedAt = now;
            }
        }
    }
}
