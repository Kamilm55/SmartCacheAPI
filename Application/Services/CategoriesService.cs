using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ICategoryMapper _categoryMapper;

    public CategoriesService(ICategoriesRepository categoriesRepository, ICategoryMapper categoryMapper)
    {
        _categoriesRepository = categoriesRepository;
        _categoryMapper = categoryMapper;
    }

    public async Task<List<CategoryResponseWithoutChildren>> GetAllAsync()
    {
        var categories = await _categoriesRepository.GetAllAsync();

        return categories.Select(ct => _categoryMapper.ToResponse(ct)).ToList();
    }

    public async Task<CategoryResponseWithoutChildren> GetByIdAsync(int id)
    {
        var  category = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        return _categoryMapper.ToResponse(category);
    }
    
    // Include all children
    public async Task<CategoryResponse> GetByIdWithChildrenAsync(int id)
    {
        var  category = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        var childrenTree = await _categoriesRepository.GetChildrenTreeAsync(category.Id);
        
        return _categoryMapper.ToResponse(category, childrenTree, new HashSet<int>());

    }

    public async Task<CategoryResponseWithoutChildren> CreateAsync(CategoryCreateRequest request)
    {
        if (request.ParentId is not null)
        {
            var existingParent = await _categoriesRepository.GetByIdAsync((int)request.ParentId)
                            ?? throw new NotFoundException(nameof(Category) +" not found with parentId:" + (int) request.ParentId);
        }

        var category =  _categoryMapper.ToEntity(request);
        
        var created = await _categoriesRepository.CreateAsync(category);
        return _categoryMapper.ToResponse(created);
    }

    public async Task<CategoryResponseWithoutChildren> UpdateAsync(int id, CategoryUpdateRequest request)
    {
        // Update parentId is not required now for PUT method
        // if (request.ParentId is not null)
        // {
        //     var existingParent = await _categoriesRepository.GetByIdAsync((int)request.ParentId)
        //                          ?? throw new NotFoundException(nameof(Category) + " not found with parentId:" + (int) request.ParentId);
        //     // if(existingParent.Id == id)
        //     //     throw new InvalidOperationException("Cannot update category ");
        // }
        var existing = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        var category = _categoryMapper.ToEntity(existing, request);

        category.LastModified = DateTime.UtcNow;
        var updatedCategory = await _categoriesRepository.UpdateAsync(category)
                                ?? throw new InvalidOperationException("Category cannot update");
        
        return _categoryMapper.ToResponse(updatedCategory);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _categoriesRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException(nameof(Category),id);

        var hasChildren = await _categoriesRepository.HasChildrenAsync(existing.Id);
        
        if (hasChildren)
        {
            throw new InvalidOperationException("Category that has children cannot delete");
        }

        await _categoriesRepository.DeleteAsync(existing.Id);
    }

}