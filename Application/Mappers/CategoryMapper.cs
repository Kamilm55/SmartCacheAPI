using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Application.Mappers;

public class CategoryMapper : ICategoryMapper
{
 
    public CategoryResponse ToResponse(Category category, List<Category> allDescendants, HashSet<int>? visited = null)
    {
        visited ??= new HashSet<int>();

        if (visited.Contains(category.Id))
            return null!; 

        visited.Add(category.Id);

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId,
            LastModified = category.LastModified ?? DateTime.UtcNow,
            IsActive = category.IsActive,
            Children = allDescendants
                .Where(c => c.ParentId == category.Id)
                .Select(c => ToResponse(c, allDescendants, visited))
                .Where(c => c != null) // Skip nulls
                .ToList()
        };
    }

    public CategoryResponseWithoutChildren ToResponse(Category category)
    {
        return new CategoryResponseWithoutChildren
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId,
            LastModified = category.LastModified,
            IsActive = category.IsActive
        };
    }

    public Category ToEntity(CategoryCreateRequest createRequest)
    {
        return new Category
        {
            Name = createRequest.Name,
            ParentId = createRequest.ParentId,
            IsActive = createRequest.IsActive,
            LastModified = null
        };
    }

    public Category ToEntity(Category category, CategoryUpdateRequest updateRequest)
    {
        category.Name = updateRequest.Name;
        category.IsActive = updateRequest.IsActive;
        category.LastModified = null;
        
        return category;
    }
}