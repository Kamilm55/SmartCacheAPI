using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Mappers.Interfaces;

public interface ITimestampMapper
{
    TimestampResponse ToTimestampResponse(
        DateTime? categoryTimestamp, bool categoryChanged,
        DateTime? serviceTimestamp, bool serviceChanged,
        DateTime? storyTimestamp, bool storyChanged
        );
}