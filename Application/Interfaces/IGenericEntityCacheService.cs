using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface IGenericEntityCacheService<T> where T : class
{
    Task<List<T>?> GetOrSetListEntityCacheAsync(Func<Task<List<T>>> fetchFromDb);

    Task<T> GetOrSetSingleEntityCacheAsync(int id, Func<Task<T?>> fetchFromDb);

    Task UpdateCacheAfterCreateAsync(T entity, int id);

    Task InvalidateCacheAsync();
    Task<DateTime?> GetLastModifiedAsync();
    Task SetSingleEntityCacheAsync(int id, T entity);
    Task DeleteKeyWithIdAsync(int id);
}