namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class CategoryListResponse
{
    public List<CategoryResponseWithoutChildren> CategoryList { get; set; } = new();
    public DateTime? LastModified { get; set; }
}
