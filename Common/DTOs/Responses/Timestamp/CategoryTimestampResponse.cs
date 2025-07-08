namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class CategoryTimestampResponse
{
    public DateTime? LastModified { get; set; }
    public bool HasChanged { get; set; }
}