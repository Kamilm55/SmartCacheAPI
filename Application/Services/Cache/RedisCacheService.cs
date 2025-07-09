using System.Text.Json;
using SmartCacheManagementSystem.Application.Interfaces;
using StackExchange.Redis;

namespace SmartCacheManagementSystem.Application.Services.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _redis;
    private readonly TimeSpan _ttl;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _redis = redis.GetDatabase();
        _logger = logger;
        _ttl = TimeSpan.FromMinutes(15);
        // _ttl = TimeSpan.FromSeconds(10); // for testing 
    }

    public async Task SetCacheAsync<T>(string key, T data)
    {
        var json = JsonSerializer.Serialize(data);
        var result = await _redis.StringSetAsync(key, json, _ttl);
        if (result)
            _logger.LogInformation("Set cache for key '{CacheKey}' with TTL {TTL}.", key, _ttl);
        else
            _logger.LogWarning("Failed to set cache for key '{CacheKey}'.", key);

    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var value = await _redis.StringGetAsync(key);
        if (value.IsNullOrEmpty)
        {
            _logger.LogDebug("Cache miss for key '{CacheKey}'.", key);
            return default; // In a generic method, default means the default value of the return type.
        }
        
        var deserialized = JsonSerializer.Deserialize<T>(value!);
        _logger.LogDebug("Cache hit for key '{CacheKey}'.", key);
        return deserialized;
    }

    public async Task DeleteCacheAsync(string key)
    {
        var deleted = await _redis.KeyDeleteAsync(key);
        if (deleted)
            _logger.LogInformation("Deleted cache for key '{CacheKey}'.", key);
        else
            _logger.LogWarning("Cache key '{CacheKey}' not found or already deleted.", key);
    }

    public async Task SetCacheAsync<T>(string categoriesData, int createdId, T created)
    {
        var cacheKey = categoriesData + ":" + createdId;
        _logger.LogTrace("Setting cache using composite key '{CacheKey}'.", cacheKey);
        await SetCacheAsync(cacheKey, created);
    }

    public async Task<T?> GetCacheAsync<T>(string categoriesData, int id)
    {
        var cacheKey = categoriesData + ":" + id;
        _logger.LogTrace("Getting cache using composite key '{CacheKey}'.", cacheKey);
        return await GetCacheAsync<T>(cacheKey);
    }

    public async Task DeleteCacheAsync(string categoriesData, int existingId)
    {
        var cacheKey = categoriesData + ":" + existingId;
        _logger.LogTrace("Deleting cache using composite key '{CacheKey}'.", cacheKey);
        await DeleteCacheAsync(cacheKey);
    }
}
