namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; }
        
    // Optional
    public IEnumerable<CategoryResponse>? Children { get; set; }
}