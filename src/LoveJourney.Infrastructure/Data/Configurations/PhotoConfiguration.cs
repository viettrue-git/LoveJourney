using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
        builder.Property(p => p.FileName).HasMaxLength(255).IsRequired();
        builder.Property(p => p.StoragePath).HasMaxLength(512).IsRequired();
        builder.Property(p => p.ThumbnailPath).HasMaxLength(512);
        builder.Property(p => p.ContentType).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Caption).HasMaxLength(500);
        builder.Property(p => p.SortOrder).HasDefaultValue(0);
        builder.HasIndex(p => new { p.CoupleId, p.CreatedAt }).IsDescending(false, true);
        builder.HasIndex(p => p.JourneyId);
        builder.HasOne(p => p.Couple).WithMany(c => c.Photos).HasForeignKey(p => p.CoupleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(p => p.Journey).WithMany(j => j.Photos).HasForeignKey(p => p.JourneyId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(p => p.Place).WithMany(pl => pl.Photos).HasForeignKey(p => p.PlaceId).OnDelete(DeleteBehavior.NoAction);
    }
}
