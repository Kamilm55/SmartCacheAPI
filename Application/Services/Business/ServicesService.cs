using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Utils;

namespace SmartCacheManagementSystem.Application.Services;

using Interfaces;
using Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _repository;
    private readonly IServiceMapper _serviceMapper;
    private readonly IGenericEntityCacheService<Service> _commonCacheService;

    public ServicesService(
        IServicesRepository repository,
        IServiceMapper serviceMapper,
        IGenericEntityCacheServiceFactory cacheServiceFactory)
    {
        _repository = repository;
        _serviceMapper = serviceMapper;
        _commonCacheService = cacheServiceFactory.Create<Service>(
            CacheKeys.SERVICES_DATA,
            CacheKeys.SERVICES_LASTMODIFIED
        );
    }

    public async Task<ServiceListResponse> GetAllAsync()
    {
        // If data exist in cache fetch from cache otherwise from db
        var services = await _commonCacheService.GetOrSetListEntityCacheAsync(() => _repository.GetAllAsync());
        
        var serviceResponses = services.Select( s => _serviceMapper.ToResponse(s)).ToList();
        
        DateTime? lastModified = await _commonCacheService.GetLastModifiedAsync();

        return _serviceMapper.ToListResponse(serviceResponses, lastModified);
    }

    public async Task<ServiceResponse> GetByIdAsync(int id)
    {
        var service = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));
        return _serviceMapper.ToResponse(service);
    }

    public async Task<ServiceResponse> CreateAsync(ServiceCreateRequest request)
    {
        var service = _serviceMapper.ToEntity(request);

        var created = await _repository.CreateAsync(service);
        
        await _commonCacheService.UpdateCacheAfterCreateAsync(created,created.Id);
        
        return _serviceMapper.ToResponse(created);
    }

    public async Task<ServiceResponse> UpdateAsync(int id, ServiceUpdateRequest request)
    {
        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id,() => _repository.GetByIdAsync(id));
        
        var service = _serviceMapper.ToEntity(request,existing); 
        service.LastModified = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(service)
                                ?? throw new InvalidOperationException("Service cannot update");
        
        await _commonCacheService.InvalidateCacheAsync();
        await _commonCacheService.SetSingleEntityCacheAsync(updated.Id,updated);
        
        return _serviceMapper.ToResponse(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id,() => _repository.GetByIdAsync(id));
        
        await _commonCacheService.InvalidateCacheAsync();
        await _repository.DeleteAsync(existing.Id);
        await _commonCacheService.DeleteKeyWithIdAsync(existing.Id);
    }
}
