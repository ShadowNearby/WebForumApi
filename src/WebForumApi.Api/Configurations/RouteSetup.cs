using Microsoft.Extensions.DependencyInjection;

namespace WebForumApi.Api.Configurations;

public static class RouteSetup
{
    public static IServiceCollection UseRouteSetup(
        this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        return services;
    }
}