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
using WebForumApi.Application.Common;
using WebForumApi.Infrastructure.Context;

namespace WebForumApi.Api.IntegrationTests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    // Db connection
    private const string _connString = $"Server=localhost;Database=web_forum_api;User=ubuntu;Password=yjs135790;TrustServerCertificate=True";
    private readonly MySqlConnection _dbContainer = new(_connString);

    private Respawner _respawner = default!;

    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        Client = CreateClient();
        await using IContext context = CreateContext();
        //await context.Database.MigrateAsync();
        await SetupRespawnerAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder
            .ConfigureServices(services =>
            {
                // Replace sql server context for sqlite
                List<Type> serviceTypes =
                    new()
                    {
                        typeof(DbContextOptions<ApplicationDbContext>), typeof(IContext)
                    };
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
                .UseMySql(_connString, new MySqlServerVersion(new Version(major: 8, minor: 0, build: 28)))
                .Options;
        return new ApplicationDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connString);
    }

    private async Task SetupRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(
            _connString,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = new[]
                {
                    "dbo"
                }
            }
        );
    }
}