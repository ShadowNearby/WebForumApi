using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Testcontainers.MsSql;
using WebForumApi.Application.Common;
using WebForumApi.Infrastructure.Context;

namespace WebForumApi.Api.IntegrationTests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    // Db connection
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithPassword("myHardCoreTestDb123")
        .WithName($"integration-tests-{Guid.NewGuid()}")
        .Build();

    private string _connString = default!;
    private Respawner _respawner = default!;

    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _connString = $"{_dbContainer.GetConnectionString()};TrustServerCertificate=True";
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
                .UseExceptionProcessor()
                .UseMySql(_connString, new MySqlServerVersion(new Version(8, 0, 28)))
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
                SchemasToInclude = new[] { "dbo" }
            }
        );
    }
}