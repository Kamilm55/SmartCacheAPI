namespace SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>?> GetAllAsync();
    Task<T?> GetByIdAsync(int id);

    Task<T> CreateAsync(T entity);

    Task<T?> UpdateAsync(T entity);

    Task<bool> DeleteAsync(int id);
}
