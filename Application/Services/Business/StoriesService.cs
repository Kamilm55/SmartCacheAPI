using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Utils;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services.Business;

public class StoriesService : IStoriesService
{
    private readonly IStoriesRepository _repository;
    private readonly IStoryMapper _storyMapper;
    private readonly IGenericEntityCacheService<Story> _commonCacheService;
    private readonly ILogger<StoriesService> _logger;

    public StoriesService(
        IStoriesRepository repository,
        IStoryMapper storyMapper,
        IGenericEntityCacheServiceFactory cacheServiceFactory,
        ILogger<StoriesService> logger)
    {
        _repository = repository;
        _storyMapper = storyMapper;
        _commonCacheService = cacheServiceFactory.Create<Story>(
            CacheKeys.STORIES_DATA,
            CacheKeys.STORIES_LASTMODIFIED);
        _logger = logger;
    }

    public async Task<StoryListResponse> GetAllAsync()
    {
        _logger.LogInformation("Getting all stories and last modified timestamp.");

        // If data exist in cache fetch from cache otherwise from db
        var stories = await _commonCacheService.GetOrSetListEntityCacheAsync(() => _repository.GetAllAsync());

        var storyResponses = stories.Select(st => _storyMapper.ToResponse(st)).ToList();

        DateTime? lastModified = await _commonCacheService.GetLastModifiedAsync();

        return _storyMapper.ToListResponse(storyResponses, lastModified);
    }

    public async Task<StoryResponse> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting story by ID {StoryId}.", id);

        var story = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));

        return _storyMapper.ToResponse(story);
    }

    public async Task<StoryResponse> CreateAsync(StoryCreateRequest request)
    {
        _logger.LogInformation("Creating new story.");

        var story = _storyMapper.ToEntity(request);

        var created = await _repository.CreateAsync(story);

        await _commonCacheService.UpdateCacheAfterCreateAsync(created, created.Id);

        return _storyMapper.ToResponse(created);
    }

    public async Task<StoryResponse> UpdateAsync(int id, StoryUpdateRequest request)
    {
        _logger.LogInformation("Updating story with ID {StoryId}.", id);

        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));

        var story = _storyMapper.ToEntity(request, existing);
        story.LastModified = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(story)
                    ?? throw new InvalidOperationException("Story cannot be updated");

        await _commonCacheService.InvalidateCacheAsync();
        await _commonCacheService.SetSingleEntityCacheAsync(updated.Id, updated);

        return _storyMapper.ToResponse(updated);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting story with ID {StoryId}.", id);

        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));

        await _commonCacheService.InvalidateCacheAsync();
        await _repository.DeleteAsync(existing.Id);
        await _commonCacheService.DeleteKeyWithIdAsync(existing.Id);
    }
}
