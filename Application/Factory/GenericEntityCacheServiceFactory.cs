using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Services.Cache;

namespace SmartCacheManagementSystem.Application.Factory;

public class GenericEntityCacheServiceFactory : IGenericEntityCacheServiceFactory
{
    private readonly IRedisCacheService _cache;

    public GenericEntityCacheServiceFactory(IRedisCacheService cache)
    {
        _cache = cache;
    }

    public IGenericEntityCacheService<T> Create<T>(string cacheKey, string lastModifiedKey) where T : class
    {
        return new GenericEntityCacheService<T>(_cache, cacheKey, lastModifiedKey);
    }
}
