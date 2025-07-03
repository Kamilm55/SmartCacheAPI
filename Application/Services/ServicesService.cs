using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;

namespace SmartCacheManagementSystem.Application.Services;

using Interfaces;
using Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IServiceMapper _serviceMapper;

    public ServicesService(IServicesRepository servicesRepository, IServiceMapper serviceMapper)
    {
        _servicesRepository = servicesRepository;
        _serviceMapper = serviceMapper;
    }

    public async Task<List<ServiceResponse>> GetAllAsync()
    {
        var services = await _servicesRepository.GetAllAsync();
        return services.Select( s => _serviceMapper.ToResponse(s)).ToList();
    }

    public async Task<ServiceResponse> GetByIdAsync(int id)
    {
        var service = await _servicesRepository.GetByIdAsync(id)
               ?? throw new NotFoundException(nameof(Service), id);
        return _serviceMapper.ToResponse(service);
    }

    public async Task<ServiceResponse> CreateAsync(ServiceCreateRequest request)
    {
        var service = _serviceMapper.ToEntity(request);

        var created = await _servicesRepository.CreateAsync(service);
        return _serviceMapper.ToResponse(created);
    }

    public async Task<ServiceResponse> UpdateAsync(int id, ServiceUpdateRequest request)
    {
        var existing = await _servicesRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException(nameof(Service), id);

        var updatedEntity = _serviceMapper.ToEntity(request,existing); 
        
        updatedEntity.LastModified = DateTime.UtcNow;

        var updated = await _servicesRepository.UpdateAsync(updatedEntity)
                                ?? throw new InvalidOperationException("Service cannot update");
        return _serviceMapper.ToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing =  await _servicesRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Service), id);
        
        bool isDeleted = await _servicesRepository.DeleteAsync(existing.Id);
        
        return isDeleted;
    }
}
