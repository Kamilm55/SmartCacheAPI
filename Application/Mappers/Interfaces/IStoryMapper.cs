using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers.Interfaces;

public interface IStoryMapper
{
    StoryResponse ToResponse(Story story);
    Story ToEntity(StoryCreateRequest request);
    Story ToEntity(StoryUpdateRequest request, Story entity);
}