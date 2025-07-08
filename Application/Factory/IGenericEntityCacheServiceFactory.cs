namespace SmartCacheManagementSystem.Application.Interfaces.Factory;

public interface IGenericEntityCacheServiceFactory
{
    IGenericEntityCacheService<T> Create<T>(string cacheKey, string lastModifiedKey) where T : class;
}
