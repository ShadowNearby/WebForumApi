using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebForumApi.Application.Common;

namespace WebForumApi.Api.Configurations;

public static class MigrationsSetup
{
    public static async Task Migrate(this WebApplication app)
    {
        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
        ILogger<IAssemblyMarker>? logger = scope.ServiceProvider.GetRequiredService<ILogger<IAssemblyMarker>>();
        IContext? dbContext = scope.ServiceProvider.GetRequiredService<IContext>();

        logger.LogInformation("Running migrations...");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Migrations applied succesfully");
    }
}