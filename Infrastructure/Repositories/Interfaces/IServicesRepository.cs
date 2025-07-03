using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public interface IServicesRepository
{
    Task<List<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task<Service> CreateAsync(Service service);
    Task<Service> UpdateAsync(Service service);
    Task<bool> DeleteAsync(int id);
}
