using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Extensions.Serializer;

namespace WebForumApi.Api.Configurations;

public static class SerializerSetup
{
    public static IServiceCollection AddSerializer(
        this IServiceCollection services
    )
    {
        services.AddTransient<ISerializerService, NewtonSerializerService>();
        return services;
    }
}