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

    public DbSet<Hero> Heroes { get; }
    public DbSet<Token> Tokens { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}