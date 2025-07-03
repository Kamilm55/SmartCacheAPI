using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;

namespace SmartCacheManagementSystem.Application.Services;

using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;

    public ServicesService(IServicesRepository servicesRepository)
    {
        _servicesRepository = servicesRepository;
    }

    public async Task<List<ServiceResponse>> GetAllAsync()
    {
        var services = await _servicesRepository.GetAllAsync();
        return services.Select(MapToResponse).ToList();
    }

    public async Task<ServiceResponse> GetByIdAsync(int id)
    {
        var service = await _servicesRepository.GetByIdAsync(id)
               ?? throw new NotFoundException(nameof(Service), id);
        return MapToResponse(service);
    }

    public async Task<ServiceResponse> CreateAsync(ServiceCreateRequest request)
    {
        
        // todo: Map this into entity with autoMapper
        var service = new Service
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            IsActive = request.IsActive
        };

        var created = await _servicesRepository.CreateAsync(service);
        return MapToResponse(created);
    }

    public async Task<ServiceResponse> UpdateAsync(int id, ServiceUpdateRequest request)
    {
        var existing = await _servicesRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException(nameof(Service), id);

        // todo: map with mapper
        existing.Name = request.Name;
        existing.Price = request.Price;
        existing.Description = request.Description;
        existing.IsActive = request.IsActive;
        existing.LastModified = DateTime.UtcNow;

        var updated = await _servicesRepository.UpdateAsync(existing);
        return MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool isDeleted = await _servicesRepository.DeleteAsync(id);
        if(!isDeleted){
            throw new InvalidOperationException($"Service with id:{id} was not deleted");
        }
        
        return isDeleted;
    }

    private static ServiceResponse MapToResponse(Service service)
    {
        return new ServiceResponse
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price,
            Description = service.Description,
            LastModified = service.LastModified,
            IsActive = service.IsActive
        };
    }
}
