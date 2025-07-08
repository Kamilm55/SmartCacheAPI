using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers.Interfaces;

public interface ICategoryMapper
{
    CategoryResponse ToResponse(Category category, List<Category> allDescendants, HashSet<int>? visited = null);
    CategoryResponseWithoutChildren ToResponse(Category category);
    Category ToEntity(CategoryCreateRequest createRequest);
    Category ToEntity(Category category, CategoryUpdateRequest updateRequest);
    CategoryListResponse ToListResponse(List<CategoryResponseWithoutChildren> categoryList, DateTime? lastModified);
}