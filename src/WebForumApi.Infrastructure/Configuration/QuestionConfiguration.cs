using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Infrastructure.Configuration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasOne(q => q.CreateUser).WithMany(u => u.CreateQuestions).HasForeignKey(u => u.CreateUserId).IsRequired();
        builder.Property(q => q.Title).IsRequired().HasMaxLength(128);
        builder.Property(q => q.Content).IsRequired();
        builder.ToTable("question");
    }
}

public class UserQuestionActionConfiguration : IEntityTypeConfiguration<UserQuestionAction>
{
    public void Configure(EntityTypeBuilder<UserQuestionAction> builder)
    {
        builder.HasKey(u => new
        {
            u.QuestionId, u.UserId
        });
        builder.HasOne(t => t.Question).WithMany(a => a.UserQuestionActions).HasForeignKey(u => u.QuestionId).IsRequired();
        builder.HasOne(t => t.User).WithMany(a => a.UserQuestionActions).HasForeignKey(u => u.UserId).IsRequired();

        builder.ToTable("user_question_action");
    }
}