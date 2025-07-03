using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers.Interfaces;

public interface IServiceMapper
{
    ServiceResponse ToResponse(Service service);
    Service ToEntity(ServiceCreateRequest request);
    Service ToEntity(ServiceUpdateRequest request, Service entity);
}
