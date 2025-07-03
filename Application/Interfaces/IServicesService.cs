using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface IServicesService
{
    Task<List<ServiceResponse>> GetAllAsync();
    Task<ServiceResponse> GetByIdAsync(int id);
    Task<ServiceResponse> CreateAsync(ServiceCreateRequest request);
    Task<ServiceResponse> UpdateAsync(int id, ServiceUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}