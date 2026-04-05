using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoveJourney.Infrastructure.Data.Configurations;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("blog_posts");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasDefaultValueSql("NEWID()");
        builder.Property(b => b.Title).HasMaxLength(300).IsRequired();
        builder.Property(b => b.Content).IsRequired();
        builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(b => b.CoverImageUrl).HasMaxLength(500);
        builder.HasIndex(b => new { b.Status, b.PublishedAt }).IsDescending(false, true);
        builder.HasOne(b => b.Couple).WithMany(c => c.BlogPosts).HasForeignKey(b => b.CoupleId).OnDelete(DeleteBehavior.Cascade);
    }
}
