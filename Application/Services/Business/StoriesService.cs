using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Utils;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _repository;
    private readonly IStoryMapper _storyMapper;
    private readonly IGenericEntityCacheService<Story> _commonCacheService;

    public StoriesService(
        IStoriesRepository repository,
        IStoryMapper storyMapper,
        IGenericEntityCacheServiceFactory cacheServiceFactory)
    {
        _repository = repository;
        _storyMapper = storyMapper;
        _commonCacheService = cacheServiceFactory.Create<Story>(
            CacheKeys.STORIES_DATA,
            CacheKeys.STORIES_LASTMODIFIED);
    }

    public async Task<StoryListResponse> GetAllAsync()
    {
        // If data exist in cache fetch from cache otherwise from db
        var stories = await _commonCacheService.GetOrSetListEntityCacheAsync(() => _repository.GetAllAsync());

        var storyResponses = stories.Select( st => _storyMapper.ToResponse(st)).ToList();

        DateTime? lastModified = await _commonCacheService.GetLastModifiedAsync();
        
        return _storyMapper.ToListResponse(storyResponses, lastModified);
    }

    public async Task<StoryResponse> GetByIdAsync(int id)
    {
        var story = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));
 
        return _storyMapper.ToResponse(story);            
    }

    public async Task<StoryResponse> CreateAsync(StoryCreateRequest request)
    {
        var story = _storyMapper.ToEntity(request);

        var created = await _repository.CreateAsync(story);
        
        await _commonCacheService.UpdateCacheAfterCreateAsync(created,created.Id);
        
        return _storyMapper.ToResponse(created);
    }

    public async Task<StoryResponse> UpdateAsync(int id, StoryUpdateRequest request)
    {
        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id,() => _repository.GetByIdAsync(id));

        var story = _storyMapper.ToEntity(request, existing);
        story.LastModified = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(story)
                    ?? throw new InvalidOperationException("Story cannot be updated");
        
        await _commonCacheService.InvalidateCacheAsync();
        await _commonCacheService.SetSingleEntityCacheAsync(updated.Id,updated);

        return _storyMapper.ToResponse(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id,() => _repository.GetByIdAsync(id));
        
        await _commonCacheService.InvalidateCacheAsync();
        await _repository.DeleteAsync(existing.Id);
        await _commonCacheService.DeleteKeyWithIdAsync(existing.Id);
    }
}