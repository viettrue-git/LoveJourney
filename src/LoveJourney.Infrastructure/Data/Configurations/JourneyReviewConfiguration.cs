using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class JourneyReviewConfiguration : IEntityTypeConfiguration<JourneyReview>
{
    public void Configure(EntityTypeBuilder<JourneyReview> builder)
    {
        builder.ToTable("journey_reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("NEWID()");
        builder.Property(r => r.Rating).IsRequired();
        builder.HasIndex(r => r.JourneyId).IsUnique();
        builder.HasOne(r => r.Journey).WithOne(j => j.Review).HasForeignKey<JourneyReview>(r => r.JourneyId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Couple).WithMany().HasForeignKey(r => r.CoupleId).OnDelete(DeleteBehavior.Restrict);
    }
}
