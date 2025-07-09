using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Utils;

namespace SmartCacheManagementSystem.Application.Services;

public class TimestampService : ITimestampService
{
    private readonly IRedisCacheService _redisCacheService;
    private readonly ITimestampMapper _mapper;
    private readonly ILogger<TimestampService> _logger;

    public TimestampService(
        IRedisCacheService redisCacheService,
        ILogger<TimestampService> logger,
        ITimestampMapper mapper)
    {
        _redisCacheService = redisCacheService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TimestampResponse> VerifyChanges(TimestampRequest request)
    {
        _logger.LogInformation("Verifying timestamp changes...");

        var categoryTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.CATEGORIES_LASTMODIFIED);
        _logger.LogDebug("Fetched category last modified timestamp from cache: {Timestamp}", categoryTimestamp);

        var serviceTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.SERVICES_LASTMODIFIED);
        _logger.LogDebug("Fetched service last modified timestamp from cache: {Timestamp}", serviceTimestamp);

        var storyTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.STORIES_LASTMODIFIED);
        _logger.LogDebug("Fetched story last modified timestamp from cache: {Timestamp}", storyTimestamp);

        var categoryChanged = HasChanged(request.CategoryTimestamp, categoryTimestamp);
        var serviceChanged = HasChanged(request.ServiceTimestamp, serviceTimestamp);
        var storyChanged = HasChanged(request.StoryTimestamp, storyTimestamp);

        _logger.LogInformation("Change detection results - Category: {CategoryChanged}, Service: {ServiceChanged}, Story: {StoryChanged}",
            categoryChanged, serviceChanged, storyChanged);

      return  _mapper.ToTimestampResponse(
            categoryTimestamp,categoryChanged,
            serviceTimestamp,serviceChanged,
            storyTimestamp,storyChanged
            );
    }

    private bool HasChanged(DateTime? requestTimestamp, DateTime? cachedTimestamp)
    {
        var changed = requestTimestamp != cachedTimestamp;
        _logger.LogTrace("Comparing timestamps: request = {RequestTimestamp}, cached = {CachedTimestamp}, changed = {Changed}",
            requestTimestamp, cachedTimestamp, changed);
        return changed;
    }
}