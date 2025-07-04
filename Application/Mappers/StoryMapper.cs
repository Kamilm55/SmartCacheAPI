using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers;

public class StoryMapper : IStoryMapper
{
    public StoryResponse ToResponse(Story story)
    {
        return new StoryResponse
        {
            Id = story.Id,
            Title = story.Title,
            Content = story.Content,
            LastModified = story.LastModified,
            IsPublished = story.IsPublished,
            ImageUrl = story.ImageUrl
        };
    }

    public Story ToEntity(StoryCreateRequest request)
    {
        return new Story
        {
            Title = request.Title,
            Content = request.Content,
            IsPublished = request.IsPublished,
            ImageUrl = request.ImageUrl
        };
    }

    public Story ToEntity(StoryUpdateRequest request, Story entity)
    {
        entity.Title = request.Title;
        entity.Content = request.Content;
        entity.IsPublished = request.IsPublished;
        entity.ImageUrl = request.ImageUrl;
        
        return entity;
    }
}
