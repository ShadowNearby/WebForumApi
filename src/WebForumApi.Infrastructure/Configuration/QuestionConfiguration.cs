using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasMany(q => q.LikeUsers).WithMany(u => u.LikeQuestions).UsingEntity("UserLikeQuestion");
        builder.HasMany(q => q.DislikeUsers).WithMany(u => u.DislikeQuestions).UsingEntity("UserDislikeQuestion");
        builder.HasMany(q => q.StarUsers).WithMany(u => u.StarQuestions).UsingEntity("UserStarQuestion");
        builder.HasOne(q => q.CreateUser).WithMany(u => u.CreateQuestions).HasForeignKey(u => u.CreateUserId).IsRequired();
        builder.Property(q => q.Title).IsRequired().HasMaxLength(128);
        builder.Property(q => q.Content).IsRequired();
    }
}