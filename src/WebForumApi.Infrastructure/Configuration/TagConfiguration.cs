using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasMany(t => t.Questions).WithMany(q => q.Tags).UsingEntity("QuestionTag");
        builder.Property(q => q.Content).IsRequired().HasMaxLength(128);
    }
}