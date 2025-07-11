using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface ICategoriesService
{
    Task<CategoryListResponse> GetAllAndLastModifiedAsync();
    Task<CategoryResponseWithoutChildren> GetByIdAsync(int id);
    Task<CategoryResponseWithoutChildren> CreateAsync(CategoryCreateRequest categoryCreateRequest);
    Task<CategoryResponseWithoutChildren> UpdateAsync(int id, CategoryUpdateRequest categoryUpdateRequest);
    Task DeleteAsync(int id);
    
    Task<CategoryResponse> GetByIdWithChildrenAsync(int id);
    
    Task<CategoryResponseWithoutChildren> CreateWithStoredProcedureAsync(CategoryCreateRequest categoryCreateRequest);

}