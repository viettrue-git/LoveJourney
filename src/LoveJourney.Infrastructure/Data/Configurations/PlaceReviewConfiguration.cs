using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class PlaceReviewConfiguration : IEntityTypeConfiguration<PlaceReview>
{
    public void Configure(EntityTypeBuilder<PlaceReview> builder)
    {
        builder.ToTable("place_reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("NEWID()");
        builder.Property(r => r.Rating).IsRequired();
        builder.HasIndex(r => r.PlaceId).IsUnique();
        builder.HasOne(r => r.Place).WithOne(p => p.Review).HasForeignKey<PlaceReview>(r => r.PlaceId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Couple).WithMany().HasForeignKey(r => r.CoupleId).OnDelete(DeleteBehavior.Restrict);
    }
}
