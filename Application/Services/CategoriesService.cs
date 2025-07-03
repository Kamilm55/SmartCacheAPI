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

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        var categories = await _categoriesRepository.GetAllAsync();

        return categories.Select(ct => _categoryMapper.ToResponse(ct)).ToList();
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var  category = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        // todo: write mapper for merging with the children
        return _categoryMapper.ToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest categoryCreateRequest)
    {
        var category =  _categoryMapper.ToEntity(categoryCreateRequest);

        // todo: consider FK for join if not exist with parent id exception then join children in dto
        var created = await _categoriesRepository.CreateAsync(category);
        
        return _categoryMapper.ToResponse(created);
    }

    public async Task<CategoryResponse> UpdateAsync(int id, CategoryUpdateRequest categoryUpdateRequest)
    {
        var existing = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        var category = _categoryMapper.ToEntity(existing, categoryUpdateRequest);
        // todo: consider FK for join if not exist with parent id exception then join children in dto

        category.LastModified = DateTime.UtcNow;
        var updatedCategory = await _categoriesRepository.UpdateAsync(category)
                                ?? throw new InvalidOperationException("Category cannot update");
        
        return _categoryMapper.ToResponse(updatedCategory);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _categoriesRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException(nameof(Category),id);

        await _categoriesRepository.DeleteAsync(existing.Id);
    }
}