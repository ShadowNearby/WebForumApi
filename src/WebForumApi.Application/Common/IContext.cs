using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }

    public DbSet<User> Users { get; }
    public DbSet<Question> Questions { get; }
    public DbSet<QuestionTag> QuestionTags { get; }
    public DbSet<Answer> Answers { get; }
    public DbSet<Tag> Tags { get; }
    public DbSet<Field> Fields { get; }

    public DbSet<Token> Tokens { get; }
    public DbSet<UserQuestionAction> UserQuestionActions { get; }
    public DbSet<UserAnswerAction> UserAnswerActions { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}