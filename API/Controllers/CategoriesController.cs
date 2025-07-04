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
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseWithoutChildren>>>> GetAll()
    {
        var categoryResponses = await _categoriesService.GetAllAsync();
        return ApiResponse<IEnumerable<CategoryResponseWithoutChildren>>.Ok(categoryResponses);
    }
    
    // GET: api/v1/categories/id/tree
    [HttpGet("{id:int}/tree")]
    public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetByIdWithChildren(int id)
    {
        var categoryResponses = await _categoriesService.GetByIdWithChildrenAsync(id);
        return ApiResponse<CategoryResponse>.Ok(categoryResponses);
    }

    // GET: api/v1/categories/id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponseWithoutChildren>>> GetById(int id)
    {
        var categoryResponse = await _categoriesService.GetByIdAsync(id);
        return ApiResponse<CategoryResponseWithoutChildren>.Ok(categoryResponse);
    }

    // POST: api/v1/categories
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryResponseWithoutChildren>>> Create(CategoryCreateRequest categoryCreateRequest)
    {
        var createdCategory = await _categoriesService.CreateAsync(categoryCreateRequest);
        return ApiResponse<CategoryResponseWithoutChildren>.Created(createdCategory, nameof(GetById) + new { id = createdCategory.Id });
    }

    // PUT: api/v1/categories/id
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CategoryResponseWithoutChildren>>> Update(int id, CategoryUpdateRequest categoryUpdateRequest)
    {
        var updatedCategory = await _categoriesService.UpdateAsync(id, categoryUpdateRequest);
        return ApiResponse<CategoryResponseWithoutChildren>.Ok(updatedCategory, "Updated category id: " + updatedCategory.Id);
    }

    // DELETE: api/v1/categories/id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        await _categoriesService.DeleteAsync(id);
        return ApiResponse<string>.NoContent($"Category with id:{id} deleted successfully");
    }
}
