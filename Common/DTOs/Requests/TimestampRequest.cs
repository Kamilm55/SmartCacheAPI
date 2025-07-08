namespace SmartCacheManagementSystem.Common.DTOs.Requests;

public class TimestampRequest
{
    public DateTime? CategoryTimestamp { get; set; }
    public DateTime? StoryTimestamp { get; set; }
    public DateTime? ServiceTimestamp { get; set; }
}