namespace SmartCacheManagementSystem.Common.DTOs.Requests.Category;

public class CategoryUpdateRequest
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; }
    public bool IsActive { get; set; }
}