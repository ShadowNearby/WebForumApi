using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Extensions.Serializer;

namespace WebForumApi.Application.Extensions.Cache;

public class DistributedCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedCacheService> _logger;
    private readonly ISerializerService _serializer;

    public DistributedCacheService(
        IDistributedCache cache,
        ISerializerService serializer,
        ILogger<DistributedCacheService> logger)
    {
        (_cache, _serializer, _logger) = (cache, serializer, logger);
    }

    public T? Get<T>(string key)
    {
        return Get(key) is { } data ? Deserialize<T>(data) : default;
    }

    private byte[]? Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        return _cache.Get(key);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default)
    {
        return await GetAsync(key, token) is { } data ? Deserialize<T>(data) : default;
    }

    private async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        return await _cache.GetAsync(key, token);
    }

    public void Refresh(string key)
    {
        _cache.Refresh(key);
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        await _cache.RefreshAsync(key, token);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await _cache.RemoveAsync(key, token);
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        Set(key, Serialize(value), slidingExpiration);
    }

    private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
    {
        _cache.Set(key, value, GetOptions(slidingExpiration));
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? slidingExpiration = null,
        CancellationToken cancellationToken = default)
    {
        return SetAsync(key, Serialize(value), slidingExpiration, cancellationToken);
    }

    private async Task SetAsync(
        string key,
        byte[] value,
        TimeSpan? slidingExpiration = null,
        CancellationToken token = default)
    {
        await _cache.SetAsync(key, value, GetOptions(slidingExpiration), token);
    }

    private byte[] Serialize<T>(T item)
    {
        return Encoding.Default.GetBytes(_serializer.Serialize(item));
    }

    private T Deserialize<T>(byte[] cachedData)
    {
        return _serializer.Deserialize<T>(Encoding.Default.GetString(cachedData));
    }

    private static DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        DistributedCacheEntryOptions? options = new();
        if (slidingExpiration.HasValue)
        {
            options.SetSlidingExpiration(slidingExpiration.Value);
        }
        else
        {
            // TODO: add to appsettings?
            // Default expiration time of 10 minutes.
            options.SetSlidingExpiration(TimeSpan.FromMinutes(10));
        }

        return options;
    }
}