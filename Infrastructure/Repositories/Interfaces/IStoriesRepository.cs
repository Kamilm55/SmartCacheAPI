using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public interface IStoriesRepository
{
    Task<List<Story>> GetAllAsync();
    Task<Story?> GetByIdAsync(int id);
    Task<Story> CreateAsync(Story story);
    Task<Story?> UpdateAsync(Story story);
    Task<bool> DeleteAsync(int id);
}