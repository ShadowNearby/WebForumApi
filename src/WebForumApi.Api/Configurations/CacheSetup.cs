using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebForumApi.Application.Extensions.Cache;

namespace WebForumApi.Api.Configurations;

public class CacheSetting
{
    public bool UseDistributedCache { get; set; }
    public bool PreferRedis { get; set; }
    public string RedisUrl { get; set; } = null!;
    public string InstanceName { get; set; } = null!;
}

public static class CacheSetup
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        CacheSetting? settings = configuration.GetSection(nameof(CacheSetting)).Get<CacheSetting>();
        if (settings == null)
        {
            return services;
        }

        if (settings.UseDistributedCache)
        {
            if (settings.PreferRedis)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = settings.RedisUrl;
                    options.InstanceName = settings.InstanceName;
                    // options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                    // {
                    //     AbortOnConnectFail = true,
                    //     EndPoints =
                    //     {
                    //         settings.RedisUrl
                    //     }
                    // };
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.AddTransient<ICacheService, DistributedCacheService>();
        }
        else
        {
            services.AddTransient<ICacheService, LocalCacheService>();
        }

        services.AddMemoryCache();
        return services;
    }
}