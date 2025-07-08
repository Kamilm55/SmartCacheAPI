using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Common.Exceptions;

namespace SmartCacheManagementSystem.Application.Services.Common;

public class GenericEntityCacheService<T> : IGenericEntityCacheService<T> where T : class
{
    private readonly IRedisCacheService _cache;
    private readonly string _cacheKey;
    private readonly string _lastModifiedKey;

    public GenericEntityCacheService(
        IRedisCacheService cache,
        string cacheKey,
        string lastModifiedKey)
    {
        _cache = cache;
        _cacheKey = cacheKey;
        _lastModifiedKey = lastModifiedKey;
    }

    // Fetch from cache, if not exist fetch from db and set to cache
    public async Task<List<T>?> GetOrSetListEntityCacheAsync(Func<Task<List<T>>> fetchFromDb)
    {
        var list = await _cache.GetCacheAsync<List<T>>(_cacheKey);
        if (list == null)
        {
            list = await fetchFromDb();
            await _cache.SetCacheAsync(_cacheKey, list); // set table cache
        }

        return list;
    }

    public async Task<T> GetOrSetSingleEntityCacheAsync(int id, Func<Task<T?>> fetchFromDb)
    {
        var entity = await _cache.GetCacheAsync<T>(_cacheKey, id);
        if (entity != null) return entity;

        entity = await fetchFromDb()
                 ?? throw new NotFoundException($"{typeof(T).Name} not found with id {id}");

        await _cache.SetCacheAsync(_cacheKey, id, entity); // set specific record into cache
        return entity;
    }

    public async Task UpdateCacheAfterCreateAsync(T entity, int id)
    {
        var list = await _cache.GetCacheAsync<List<T>>(_cacheKey);
        if (list != null)
        {
            list.Add(entity);
            await _cache.SetCacheAsync(_cacheKey, list);
        }

        await _cache.SetCacheAsync(_cacheKey, id, entity);
        await _cache.SetCacheAsync(_lastModifiedKey, DateTime.UtcNow);
    }

    public async Task InvalidateCacheAsync()
    {
        await _cache.DeleteCacheAsync(_cacheKey);
        await _cache.SetCacheAsync(_lastModifiedKey, DateTime.UtcNow);
    }

    public async Task<DateTime?> GetLastModifiedAsync()
    {
        return await _cache.GetCacheAsync<DateTime?>(_lastModifiedKey);
    }

    public async Task SetSingleEntityCacheAsync(int id, T entity)
    {
        await _cache.SetCacheAsync(_cacheKey, id, entity);
    }

    public async Task DeleteKeyWithIdAsync(int id)
    {
        await _cache.DeleteCacheAsync(_cacheKey, id);
    }
}
