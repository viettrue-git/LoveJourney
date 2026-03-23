using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class JourneyConfiguration : IEntityTypeConfiguration<Journey>
{
    public void Configure(EntityTypeBuilder<Journey> builder)
    {
        builder.ToTable("journeys");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasDefaultValueSql("NEWID()");
        builder.Property(j => j.Title).HasMaxLength(200).IsRequired();
        builder.Property(j => j.JourneyType).HasMaxLength(20).HasDefaultValue("other");
        builder.Property(j => j.JourneyDate).IsRequired();
        builder.HasIndex(j => new { j.CoupleId, j.JourneyDate }).IsDescending(false, true);
        builder.HasOne(j => j.Couple).WithMany(c => c.Journeys).HasForeignKey(j => j.CoupleId).OnDelete(DeleteBehavior.Cascade);
    }
}
