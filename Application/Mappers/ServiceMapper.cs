using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers;

public class ServiceMapper : IServiceMapper
{
    public ServiceResponse ToResponse(Service service)
    {
        if (service == null) return null!;

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

    public Service ToEntity(ServiceCreateRequest request)
    {
        if (request == null) return null!;

        return new Service
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            IsActive = request.IsActive
        };
    }

    public Service ToEntity(ServiceUpdateRequest request, Service entity)
    {
        if (request == null || entity == null) return null;

        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.Description = request.Description;
        entity.IsActive = request.IsActive;
        
        return entity;
    }
}
