namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class StoryResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime LastModified { get; set; }
    public bool IsPublished { get; set; }
    public string? ImageUrl { get; set; }
}