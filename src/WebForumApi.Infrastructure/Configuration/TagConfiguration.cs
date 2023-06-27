using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasMany(t => t.Questions).WithMany(q => q.Tags).UsingEntity("question_tag");
        builder.Property(q => q.Content).IsRequired().HasMaxLength(64);
        builder.Property(q => q.Description).IsRequired().HasMaxLength(256);
        builder.ToTable("tag");
    }
}