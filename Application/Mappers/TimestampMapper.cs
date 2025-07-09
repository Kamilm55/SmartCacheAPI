using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Mappers;

public class TimestampMapper : ITimestampMapper
{
    public TimestampResponse ToTimestampResponse(
        DateTime? categoryTimestamp, bool categoryChanged,
        DateTime? serviceTimestamp, bool serviceChanged,
        DateTime? storyTimestamp, bool storyChanged)
    {
        return new TimestampResponse
        {
            Category = new CategoryTimestampResponse
            {
                LastModified = categoryTimestamp,
                HasChanged = categoryChanged
            },
            Service = new ServiceTimestampResponse
            {
                LastModified = serviceTimestamp,
                HasChanged = serviceChanged
            },
            Story = new StoryTimestampService
            {
                LastModified = storyTimestamp,
                HasChanged = storyChanged
            }
        };
    }

}