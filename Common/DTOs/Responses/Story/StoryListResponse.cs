namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class StoryListResponse
{
    public List<StoryResponse> StoryList { get; set; } = new();
    public DateTime? LastModified { get; set; }
}