using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasMany(q => q.LikeUsers).WithMany(u => u.LikeAnswers).UsingEntity("UserLikeAnswer");
        builder.HasMany(q => q.DislikeUsers).WithMany(u => u.DislikeAnswers).UsingEntity("UserDislikeAnswer");
        builder.HasMany(q => q.StarUsers).WithMany(u => u.StarAnswers).UsingEntity("UserStarAnswer");
        builder.HasOne(q => q.CreateUser).WithMany(u => u.CreateAnswers).HasForeignKey(u => u.CreateUserId).IsRequired();
        builder.Property(q => q.Content).IsRequired();
    }
}