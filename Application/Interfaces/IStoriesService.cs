using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface IStoriesService
{
    Task<List<StoryResponse>> GetAllAsync();
    Task<StoryResponse> GetByIdAsync(int id);
    Task<StoryResponse> CreateAsync(StoryCreateRequest request);
    Task<StoryResponse> UpdateAsync(int id, StoryUpdateRequest request);
    Task<bool> DeleteAsync(int id);
}