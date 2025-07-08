using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;
using SmartCacheManagementSystem.Common.Utils;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ICategoryMapper _categoryMapper;
    private readonly IRedisCacheService _redisCacheService;

    public CategoriesService(ICategoriesRepository categoriesRepository, ICategoryMapper categoryMapper, IRedisCacheService redisCacheService)
    {
        _categoriesRepository = categoriesRepository;
        _categoryMapper = categoryMapper;
        _redisCacheService = redisCacheService;
    }

    public async Task<CategoryListResponse> GetAllAndLastModifiedAsync()
    {
        // If categories data exist in cache fetch from cache otherwise from db
        List<Category>? categories = await _redisCacheService.GetCacheAsync<List<Category>>(CacheKeys.CATEGORIES_DATA);

        if (categories == null)
        {
            categories = await _categoriesRepository.GetAllAsync();
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA, categories);
        }

        var categoryResponseList = categories.Select(ct => _categoryMapper.ToResponse(ct)).ToList();
        
        DateTime? lastModified = await _redisCacheService.GetCacheAsync<DateTime?>(CacheKeys.CATEGORIES_LASTMODIFIED);
        
        return _categoryMapper.ToListResponse(categoryResponseList,lastModified);
    }

    public async Task<CategoryResponseWithoutChildren> GetByIdAsync(int id)
    {
        Category? category;
        category = await _redisCacheService.GetCacheAsync<Category>(CacheKeys.CATEGORIES_DATA,id);
        if (category == null)
        {
            category = await _categoriesRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Category), id);
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,category.Id, category);
        }
        
        return _categoryMapper.ToResponse(category);
    }
    
    // Include all children
    public async Task<CategoryResponse> GetByIdWithChildrenAsync(int id)
    {
        Category? category;
        category = await _redisCacheService.GetCacheAsync<Category>(CacheKeys.CATEGORIES_DATA,id);
        if (category == null)
        {
            category = await _categoriesRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException(nameof(Category), id);
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,category.Id, category);
        }
        var childrenTree = await _categoriesRepository.GetChildrenTreeAsync(category.Id);
        
        return _categoryMapper.ToResponse(category, childrenTree, new HashSet<int>());
    }

    public async Task<CategoryResponseWithoutChildren> CreateWithStoredProcedureAsync(CategoryCreateRequest request)
    {
        await EnsureParentExistsIfNeededAsync(request.ParentId);

        var category = _categoryMapper.ToEntity(request);
        var created = await _categoriesRepository.CreateCategoryWithStoredProcedureAsync(category);

        List<Category>? categories;
        // If cache is not null add created to cache,also change lastModified -> User can fetch from cache 
        if ((categories = await _redisCacheService.GetCacheAsync<List<Category>>(CacheKeys.CATEGORIES_DATA)) != null)
        {
            categories.Add(created);
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,categories);
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_LASTMODIFIED, DateTime.UtcNow);
        }
        // Set cache for id
        await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,created.Id, created);
                
        return _categoryMapper.ToResponse(created);
    }

    public async Task<CategoryResponseWithoutChildren> CreateAsync(CategoryCreateRequest request)
    {
        await EnsureParentExistsIfNeededAsync(request.ParentId);

        var category = _categoryMapper.ToEntity(request);
        var created = await _categoriesRepository.CreateAsync(category);
        
        List<Category>? categories;
        // If cache is not null add created to cache,but don't change lastModified -> User can fetch from cache 
        if ((categories = await _redisCacheService.GetCacheAsync<List<Category>>(CacheKeys.CATEGORIES_DATA)) != null)
        {
            categories.Add(created);
            await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,categories);
        }
        
        // Set cache for id
        await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,created.Id, created);

        return _categoryMapper.ToResponse(created);
    }

    private async Task EnsureParentExistsIfNeededAsync(int? parentId)
    {
        if (parentId == null) return;

        var existingParent = await _categoriesRepository.GetByIdAsync(parentId.Value);
        if (existingParent == null)
        {
            throw new NotFoundException(nameof(Category) +  $"not found with parentId: {parentId.Value}");
        }
    }


    public async Task<CategoryResponseWithoutChildren> UpdateAsync(int id, CategoryUpdateRequest request)
    {
        // Update parentId is not required now for PUT method
        var existing = await _categoriesRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(nameof(Category),id);
        
        var category = _categoryMapper.ToEntity(existing, request);

        category.LastModified = DateTime.UtcNow; // lastModified field of an instance
        var updatedCategory = await _categoriesRepository.UpdateAsync(category)
                                ?? throw new InvalidOperationException("Category cannot update");
        
        // Instead of iterating and update cache we change lastModified and client fetch from db next time
        await _redisCacheService.DeleteCacheAsync(CacheKeys.CATEGORIES_DATA);
        await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_LASTMODIFIED, DateTime.UtcNow);
        
        // Set cache for id
        await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_DATA,updatedCategory.Id, updatedCategory);
        
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
        
        // Instead of iterating and delete cache we change lastModified and client fetch from db next time
        await _redisCacheService.DeleteCacheAsync(CacheKeys.CATEGORIES_DATA);
        await _redisCacheService.SetCacheAsync(CacheKeys.CATEGORIES_LASTMODIFIED, DateTime.UtcNow);

        await _redisCacheService.DeleteCacheAsync(CacheKeys.CATEGORIES_DATA,existing.Id);
        await _categoriesRepository.DeleteAsync(existing.Id);
    }
}