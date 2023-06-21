using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using WebForumApi.Infrastructure.Configuration;

namespace WebForumApi.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Hero> Heroes { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}