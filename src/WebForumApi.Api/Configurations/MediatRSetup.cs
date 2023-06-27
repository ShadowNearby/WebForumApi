using Microsoft.Extensions.DependencyInjection;
using WebForumApi.Application.Common.Behaviors;

namespace WebForumApi.Api.Configurations;

public static class MediatRSetup
{
    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR(
            (config) =>
            {
                config.RegisterServicesFromAssemblyContaining(
                    typeof(WebForumApi.Application.IAssemblyMarker)
                );
                config.AddOpenBehavior(typeof(ValidationResultPipelineBehavior<,>));
            }
        );

        return services;
    }
}