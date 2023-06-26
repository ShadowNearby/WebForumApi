using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using WebForumApi.Infrastructure.Configuration;
using TokenConfiguration=WebForumApi.Infrastructure.Configuration.TokenConfiguration;

namespace WebForumApi.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Field> Fields { get; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Question> Questions { get; } = null!;
    public DbSet<Answer> Answers { get; } = null!;
    public DbSet<Tag> Tags { get; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseExceptionProcessor()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        ;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfiguration(new TokenConfiguration());
    }
}