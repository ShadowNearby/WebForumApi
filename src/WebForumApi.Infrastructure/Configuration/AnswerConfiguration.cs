using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasOne(q => q.CreateUser).WithMany(u => u.CreateAnswers).HasForeignKey(u => u.CreateUserId).IsRequired();
        builder.Property(q => q.Content).IsRequired();
        builder.Property(x => x.CreateUserUsername).IsRequired().HasMaxLength(64);
        builder.Property(x => x.CreateUserAvatar).HasMaxLength(256);
        builder.ToTable("answer");
    }
}

public class UserAnswerActionConfiguration : IEntityTypeConfiguration<UserAnswerAction>
{
    public void Configure(EntityTypeBuilder<UserAnswerAction> builder)
    {
        builder.HasOne(t => t.Answer).WithMany(a => a.UserAnswerActions).HasForeignKey(u => u.AnswerId).IsRequired();
        builder.HasOne(t => t.User).WithMany(a => a.UserAnswerActions).HasForeignKey(u => u.UserId).IsRequired();
        builder.HasKey(u => new
        {
            u.AnswerId, u.UserId
        });
        builder.ToTable("user_answer_action");
    }
}