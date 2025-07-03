namespace SmartCacheManagementSystem.Common.DTOs.Requests.Category;

public class CategoryCreateRequest
{
    public string Name { get; set; } = null!;
    public int? ParentId { get; set; } // If there is no parent by default it must be sent null
    public bool IsActive { get; set; } = true;
}