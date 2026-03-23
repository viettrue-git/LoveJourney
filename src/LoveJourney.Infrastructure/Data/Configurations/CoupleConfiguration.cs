using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class CoupleConfiguration : IEntityTypeConfiguration<Couple>
{
    public void Configure(EntityTypeBuilder<Couple> builder)
    {
        builder.ToTable("couples");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("NEWID()");
        builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
        builder.HasIndex(c => c.Email).IsUnique();
        builder.Property(c => c.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(c => c.Partner1Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Partner2Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.StartDate).IsRequired();
        builder.Property(c => c.ProfilePhotoUrl).HasMaxLength(512);
        builder.Property(c => c.Timezone).HasMaxLength(50).HasDefaultValue("UTC");
    }
}
