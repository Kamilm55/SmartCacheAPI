using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Utils;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Services;

public class TimestampService :  ITimestampService
{
    private readonly IRedisCacheService _redisCacheService;

    public TimestampService(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    public async Task<TimestampResponse> VerifyChanges(TimestampRequest request)
    {
        //todo: NULL Check , in request and from cached and create mapper
        
        // Cached timestamps
        var categoryTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.CATEGORIES_LASTMODIFIED);
        var serviceTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.SERVICES_LASTMODIFIED);
        var storyTimestamp = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.STORIES_LASTMODIFIED);
        
        CategoryTimestampResponse categoryTimestampResponse = new CategoryTimestampResponse()
        {
            LastModified = categoryTimestamp,
            HasChanged = categoryTimestamp != null 
                ? request.CategoryTimestamp.Value != categoryTimestamp.Value
                : false,
        };
        ServiceTimestampResponse serviceTimestampResponse = new ServiceTimestampResponse()
        {
            LastModified = serviceTimestamp,
            HasChanged = serviceTimestamp != null
                ? request.ServiceTimestamp.Value != request.ServiceTimestamp.Value
                : false,
        };
        StoryTimestampService storyTimestampService = new StoryTimestampService()
        {
            LastModified = storyTimestamp,
            HasChanged = storyTimestamp != null
                ? request.StoryTimestamp.Value != request.StoryTimestamp.Value
                : false,
        };

        return new TimestampResponse()
        {
            Category = categoryTimestampResponse,
            Service = serviceTimestampResponse,
            Story = storyTimestampService
        };
    }
}