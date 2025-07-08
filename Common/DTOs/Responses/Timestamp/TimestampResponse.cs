namespace SmartCacheManagementSystem.Common.DTOs.Responses;

public class TimestampResponse
{
  public CategoryTimestampResponse Category { get; set; }
  public ServiceTimestampResponse Service { get; set; }
  public StoryTimestampService Story {get;set;  }
}