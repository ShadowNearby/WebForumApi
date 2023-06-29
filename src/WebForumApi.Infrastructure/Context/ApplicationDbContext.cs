using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using WebForumApi.Infrastructure.Configuration;
using TokenConfiguration = WebForumApi.Infrastructure.Configuration.TokenConfiguration;

namespace WebForumApi.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Field> Fields { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionTag> QuestionTags { get; set; } = null!;
    public DbSet<UserQuestionAction> UserQuestionActions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<UserAnswerAction> UserAnswerActions { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            // .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfiguration(new TokenConfiguration());
    }
}