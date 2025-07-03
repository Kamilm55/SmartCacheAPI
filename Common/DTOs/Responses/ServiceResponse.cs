namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class ServiceResponse
{
    public int Id { get; set; }                    
    public string Name { get; set; } = null!;
    public float Price { get; set; }
    public string? Description { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; }
}