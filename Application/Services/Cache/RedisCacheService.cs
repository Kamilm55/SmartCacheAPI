using System.Text.Json;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Domain.Entities;
using StackExchange.Redis;

namespace SmartCacheManagementSystem.Application.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _redis;
    private readonly TimeSpan _ttl;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
       _ttl = TimeSpan.FromMinutes(15);
        // _ttl = TimeSpan.FromSeconds(10); // for testing 
    }

    public async Task SetCacheAsync<T>(string key, T data)
    {
       var json = JsonSerializer.Serialize(data);
       await _redis.StringSetAsync(key, json, _ttl);
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var value = await _redis.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default; // In a generic method, default means the default value of the return type.
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task DeleteCacheAsync(string key)
    {
        await _redis.KeyDeleteAsync(key); 
    }

    public async Task SetCacheAsync<T>(string categoriesData, int createdId, T created)
    {
        await SetCacheAsync(categoriesData + ":" + createdId, created);
    }

    public async Task<T?> GetCacheAsync<T>(string categoriesData, int id) 
    {
       return await GetCacheAsync<T>(categoriesData + ":" + id);
    }

    public async Task DeleteCacheAsync(string categoriesData, int existingId)
    {
        await DeleteCacheAsync(categoriesData + ":" + existingId);
    }
}