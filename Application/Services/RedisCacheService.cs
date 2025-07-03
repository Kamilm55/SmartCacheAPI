using Microsoft.Extensions.Options;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Infrastructure.Configuration;
using StackExchange.Redis;

namespace SmartCacheManagementSystem.Application.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly ConnectionMultiplexer _redis;

    public RedisCacheService(IOptions<RedisOptions> redisOptions)
    {
        _redis = ConnectionMultiplexer.Connect(redisOptions.Value.Configuration);
    }

    public IDatabase GetDatabase() => _redis.GetDatabase();
}