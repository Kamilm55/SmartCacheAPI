using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class ServiceListResponse
{
    public List<ServiceResponse> ServiceList { get; set; } = new();
    public DateTime? LastModified { get; set; }
}