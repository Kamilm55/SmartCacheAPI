using Microsoft.AspNetCore.Mvc;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.DTOs.Requests.Story;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StoriesController : ControllerBase
{
    private readonly IStoriesService _storiesService;

    public StoriesController(IStoriesService storiesService)
    {
        _storiesService = storiesService;
    }

    // GET: api/v1/stories
    [HttpGet]
    public async Task<ActionResult<ApiResponse<StoryListResponse>>> GetAll()
    {
        var storyResponses = await _storiesService.GetAllAsync();
        return ApiResponse<StoryListResponse>.Ok(storyResponses);
    }

    // GET: api/v1/stories/id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<StoryResponse>>> GetById(int id)
    {
        var storyResponse = await _storiesService.GetByIdAsync(id);
        return ApiResponse<StoryResponse>.Ok(storyResponse);
    }

    // POST: api/v1/stories
    [HttpPost]
    public async Task<ActionResult<ApiResponse<StoryResponse>>> Create(StoryCreateRequest storyCreateRequest)
    {
        var createdStory = await _storiesService.CreateAsync(storyCreateRequest);
        return ApiResponse<StoryResponse>.Created(createdStory, nameof(GetById) + new { id = createdStory.Id });
    }

    // PUT: api/v1/stories/id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<StoryResponse>>> Update(int id, StoryUpdateRequest storyUpdateRequest)
    {
        var updatedStory = await _storiesService.UpdateAsync(id, storyUpdateRequest);
        return ApiResponse<StoryResponse>.Ok(updatedStory, "Updated story id: " + updatedStory.Id);
    }

    // DELETE: api/v1/stories/id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        await _storiesService.DeleteAsync(id);
        return ApiResponse<string>.NoContent($"Story with id:{id} deleted successfully");
    }
}
