namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class ServiceTimestampResponse
{
    public DateTime? LastModified { get; set; }
    public bool HasChanged { get; set; }
}