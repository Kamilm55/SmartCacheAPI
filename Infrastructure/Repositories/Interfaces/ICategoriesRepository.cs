using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public interface ICategoriesRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
    
    Task<List<Category>> GetChildrenAsync(int parentId);
}