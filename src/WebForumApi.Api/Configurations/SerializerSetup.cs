using Microsoft.Extensions.DependencyInjection;
using WebForumApi.Application.Extensions.Serializer;

namespace WebForumApi.Api.Configurations;

public static class SerializerSetup
{
    public static IServiceCollection AddSerializer(
        this IServiceCollection services
    )
    {
        services.AddTransient<ISerializerService, SystemSerializerService>();
        return services;
    }
}