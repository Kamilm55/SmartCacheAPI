using Microsoft.AspNetCore.Mvc;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    // GET: api/v1/categories
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponse>>>> GetAll()
    {
        var categoryResponses = await _categoriesService.GetAllAsync();
        return ApiResponse<IEnumerable<CategoryResponse>>.Ok(categoryResponses);
    }

    // GET: api/v1/categories/id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetById(int id)
    {
        var categoryResponse = await _categoriesService.GetByIdAsync(id);
        return ApiResponse<CategoryResponse>.Ok(categoryResponse);
    }

    // POST: api/v1/categories
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> Create(CategoryCreateRequest categoryCreateRequest)
    {
        var createdCategory = await _categoriesService.CreateAsync(categoryCreateRequest);
        return ApiResponse<CategoryResponse>.Created(createdCategory, nameof(GetById) + new { id = createdCategory.Id });
    }

    // PUT: api/v1/categories/id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> Update(int id, CategoryUpdateRequest categoryUpdateRequest)
    {
        var updatedCategory = await _categoriesService.UpdateAsync(id, categoryUpdateRequest);
        return ApiResponse<CategoryResponse>.Ok(updatedCategory, "Updated category id: " + updatedCategory.Id);
    }

    // DELETE: api/v1/categories/id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        await _categoriesService.DeleteAsync(id);
        return ApiResponse<string>.NoContent($"Category with id:{id} deleted successfully");
    }
}
