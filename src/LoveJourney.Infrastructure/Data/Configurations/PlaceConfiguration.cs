using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.ToTable("places");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.Latitude).HasPrecision(10, 7);
        builder.Property(p => p.Longitude).HasPrecision(10, 7);
        builder.Property(p => p.GooglePlaceId).HasMaxLength(300);
        builder.Property(p => p.Category).HasMaxLength(50);
        builder.HasIndex(p => p.JourneyId);
        builder.HasOne(p => p.Journey).WithMany(j => j.Places).HasForeignKey(p => p.JourneyId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(p => p.Couple).WithMany().HasForeignKey(p => p.CoupleId).OnDelete(DeleteBehavior.Restrict);
    }
}
