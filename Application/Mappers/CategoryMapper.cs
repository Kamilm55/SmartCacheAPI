using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers;

public class CategoryMapper : ICategoryMapper
{
 
    public CategoryResponse ToResponse(Category category)
    {
        if (category == null) return null!;
            
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId,
            LastModified = category.LastModified,
            IsActive = category.IsActive,
            Children = category.Children?.Select(ToResponse).ToList()
        };
    }

    public Category ToEntity(CategoryCreateRequest createRequest)
    {
        if (createRequest == null) return null!;
            
        return new Category
        {
            Name = createRequest.Name,
            ParentId = createRequest.ParentId,
            IsActive = createRequest.IsActive
        };
    }

    public Category ToEntity(Category category, CategoryUpdateRequest updateRequest)
    {
        if (category == null || updateRequest == null) return null;

        category.Name = updateRequest.Name;
        category.ParentId = updateRequest.ParentId;
        category.IsActive = updateRequest.IsActive;
        
        return category;
    }
}