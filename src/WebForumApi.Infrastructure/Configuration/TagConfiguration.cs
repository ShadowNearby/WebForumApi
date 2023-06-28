using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(q => q.Content).IsRequired().HasMaxLength(64);
        builder.Property(q => q.Description).IsRequired().HasMaxLength(256);
        builder.ToTable("tag");
    }
}

public class QuestionTagConfiguration : IEntityTypeConfiguration<QuestionTag>
{
    public void Configure(EntityTypeBuilder<QuestionTag> builder)
    {
        builder.HasKey(qt => new
        {
            qt.QuestionId, qt.TagId
        });
        builder.HasOne(t => t.Question).WithMany(q => q.QuestionTags).HasForeignKey(a => a.QuestionId).IsRequired();
        builder.HasOne(t => t.Tag).WithMany(q => q.QuestionTags).HasForeignKey(a => a.TagId).IsRequired();
        builder.ToTable("question_tag");
    }
}