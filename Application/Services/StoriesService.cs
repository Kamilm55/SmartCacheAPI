using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _storiesRepository;
    private readonly IStoryMapper _storyMapper;

    public StoriesService(IStoriesRepository storiesRepository, IStoryMapper storyMapper)
    {
        _storiesRepository = storiesRepository;
        _storyMapper = storyMapper;
    }

    public async Task<List<StoryResponse>> GetAllAsync()
    {
        var stories = await _storiesRepository.GetAllAsync();
        
        return  stories.Select( st => _storyMapper.ToResponse(st)).ToList();
    }

    public async Task<StoryResponse> GetByIdAsync(int id)
    {
        var story = await _storiesRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Story),id);
 
        return _storyMapper.ToResponse(story);            
    }

    public async Task<StoryResponse> CreateAsync(StoryCreateRequest request)
    {
        var story = _storyMapper.ToEntity(request);

        var created = await _storiesRepository.CreateAsync(story);
        
        return _storyMapper.ToResponse(created);
    }

    public async Task<StoryResponse> UpdateAsync(int id, StoryUpdateRequest request)
    {
        var existing = await _storiesRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException(nameof(Story),id);

        var story = _storyMapper.ToEntity(request, existing);

        story.LastModified = DateTime.UtcNow;
        var updated = await _storiesRepository.UpdateAsync(story)
                    ?? throw new InvalidOperationException("Story cannot be updated");
        
        return _storyMapper.ToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var story = await _storiesRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException(nameof(Story),id);

        return await _storiesRepository.DeleteAsync(story.Id);
    }
}