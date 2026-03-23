using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class AnniversaryConfiguration : IEntityTypeConfiguration<Anniversary>
{
    public void Configure(EntityTypeBuilder<Anniversary> builder)
    {
        builder.ToTable("anniversaries");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasDefaultValueSql("NEWID()");
        builder.Property(a => a.Title).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Date).IsRequired();
        builder.Property(a => a.Recurrence).HasMaxLength(20).HasDefaultValue("none");
        builder.Property(a => a.ReminderDaysBefore).HasDefaultValue(1);
        builder.Property(a => a.IsActive).HasDefaultValue(true);
        builder.HasIndex(a => new { a.CoupleId, a.Date });
        builder.HasOne(a => a.Couple).WithMany(c => c.Anniversaries).HasForeignKey(a => a.CoupleId).OnDelete(DeleteBehavior.Cascade);
    }
}
