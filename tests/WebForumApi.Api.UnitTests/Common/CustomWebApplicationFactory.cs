using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebForumApi.Application;
using WebForumApi.Application.Common;
using WebForumApi.Infrastructure.Context;
using Xunit;

namespace WebForumApi.Api.UnitTests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    // Db connection
    private static readonly string ConnectionString =
        "server=localhost;database=webforum;user=root;password=meg20030129;";

    private readonly MySqlConnection _dbConnection = new(ConnectionString);

    private Respawner _respawner = default!;

    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbConnection.OpenAsync();
        Client = CreateClient();
        await using IContext context = CreateContext();
        await context.Database.MigrateAsync();
        await SetupRespawnerAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder
            .ConfigureServices(services =>
            {
                // Replace sql server context for sqlite
                List<Type> serviceTypes =
                    new() { typeof(DbContextOptions<ApplicationDbContext>), typeof(IContext) };
                List<ServiceDescriptor> contextsDescriptor = services
                    .Where(d => serviceTypes.Contains(d.ServiceType))
                    .ToList();
                foreach (ServiceDescriptor descriptor in contextsDescriptor)
                {
                    services.Remove(descriptor);
                }

                services.AddScoped(_ => CreateContext());
            })
            .ConfigureLogging(o => o.AddFilter(loglevel => loglevel >= LogLevel.Error));
        base.ConfigureWebHost(builder);
    }

    public IContext CreateContext()
    {
        DbContextOptions<ApplicationDbContext> options =
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseMySql(ConnectionString, new MySqlServerVersion(new Version(major: 8, minor: 0, build: 33)))
                .Options;
        return new ApplicationDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task SetupRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions { DbAdapter = DbAdapter.MySql, SchemasToInclude = new[] { "web_forum" } }
        );
    }
}