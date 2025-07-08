using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public interface ICategoriesRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);

    Task<List<Category>> GetChildrenTreeAsync(int parentId, HashSet<int>? visited = null);
    Task<bool> HasChildrenAsync(int id);

    Task<Category> CreateCategoryWithStoredProcedureAsync(Category category);
}