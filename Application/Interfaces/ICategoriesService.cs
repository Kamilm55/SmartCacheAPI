using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface ICategoriesService
{
    Task<List<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse> GetByIdAsync(int id);
    Task<CategoryResponse> CreateAsync(CategoryCreateRequest categoryCreateRequest);
    Task<CategoryResponse> UpdateAsync(int id, CategoryUpdateRequest categoryUpdateRequest);
    Task DeleteAsync(int id);
}