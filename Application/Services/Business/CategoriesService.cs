using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Common.DTOs.Requests.Category;
using SmartCacheManagementSystem.Common.DTOs.Responses;
using SmartCacheManagementSystem.Common.Exceptions;
using SmartCacheManagementSystem.Common.Utils;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Application.Services.Business;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository _repository;
    private readonly ICategoryMapper _categoryMapper;
    private readonly IGenericEntityCacheService<Category> _commonCacheService;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(
        ICategoriesRepository repository,
        ICategoryMapper categoryMapper,
        IGenericEntityCacheServiceFactory cacheServiceFactory,
        ILogger<CategoriesService> logger)
    {
        _repository = repository;
        _categoryMapper = categoryMapper;
        _commonCacheService = cacheServiceFactory.Create<Category>(
            CacheKeys.CATEGORIES_DATA,
            CacheKeys.CATEGORIES_LASTMODIFIED
        );
        _logger = logger;
    }

    public async Task<CategoryListResponse> GetAllAndLastModifiedAsync()
    {
        _logger.LogInformation("Getting all categories and last modified timestamp.");

        // If categories data exist in cache fetch from cache otherwise from db
        var categories = await _commonCacheService.GetOrSetListEntityCacheAsync(() => _repository.GetAllAsync());

        var categoryResponseList = categories.Select(ct => _categoryMapper.ToResponse(ct)).ToList();

        DateTime? lastModified = await _commonCacheService.GetLastModifiedAsync();

        return _categoryMapper.ToListResponse(categoryResponseList, lastModified);
    }

    public async Task<CategoryResponseWithoutChildren> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting category by ID {CategoryId}.", id);

        // If exists in cache fetch,otherwise fetch from db and set to cache
        var category = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));
        return _categoryMapper.ToResponse(category);
    }

    // Include all children
    public async Task<CategoryResponse> GetByIdWithChildrenAsync(int id)
    {
        _logger.LogInformation("Getting category with children by ID {CategoryId}.", id);

        var category = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));
        var childrenTree = await _repository.GetChildrenTreeAsync(category.Id);

        return _categoryMapper.ToResponse(category, childrenTree, new HashSet<int>());
    }

    public async Task<CategoryResponseWithoutChildren> CreateWithStoredProcedureAsync(CategoryCreateRequest request)
    {
        _logger.LogInformation("Creating category with stored procedure. ParentId: {ParentId}", request.ParentId);

        await EnsureParentExistsIfNeededAsync(request.ParentId);

        var category = _categoryMapper.ToEntity(request);
        var created = await _repository.CreateCategoryWithStoredProcedureAsync(category);

        await _commonCacheService.UpdateCacheAfterCreateAsync(created, created.Id);

        return _categoryMapper.ToResponse(created);
    }

    public async Task<CategoryResponseWithoutChildren> CreateAsync(CategoryCreateRequest request)
    {
        _logger.LogInformation("Creating category. ParentId: {ParentId}", request.ParentId);

        await EnsureParentExistsIfNeededAsync(request.ParentId);

        var category = _categoryMapper.ToEntity(request);
        var created = await _repository.CreateAsync(category);

        await _commonCacheService.UpdateCacheAfterCreateAsync(created, created.Id);

        return _categoryMapper.ToResponse(created);
    }
    
    public async Task<CategoryResponseWithoutChildren> UpdateAsync(int id, CategoryUpdateRequest request)
    {
        _logger.LogInformation("Updating category with ID {CategoryId}.", id);

        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));

        var category = _categoryMapper.ToEntity(existing, request);
        category.LastModified = DateTime.UtcNow; // lastModified field of an instance

        var updated = await _repository.UpdateAsync(category)
                      ?? throw new InvalidOperationException("Category cannot update");

        // Instead of iterating and update cache we change lastModified and client fetch from db next time
        await _commonCacheService.InvalidateCacheAsync();
        await _commonCacheService.SetSingleEntityCacheAsync(updated.Id, updated);
        
        return _categoryMapper.ToResponse(updated);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Deleting category with ID {CategoryId}.", id);

        var existing = await _commonCacheService.GetOrSetSingleEntityCacheAsync(id, () => _repository.GetByIdAsync(id));
        
        var hasChildren = await _repository.HasChildrenAsync(existing.Id);

        if (hasChildren)
        {
            _logger.LogWarning("Attempted to delete category with ID {CategoryId} that has children. Operation aborted.", id);
            throw new InvalidOperationException("Category that has children cannot delete");
        }

        // Instead of iterating and delete cache we change lastModified and client fetch from db next time
        await _commonCacheService.InvalidateCacheAsync();
        await _repository.DeleteAsync(existing.Id);
        await _commonCacheService.DeleteKeyWithIdAsync(existing.Id);
    }

    private async Task EnsureParentExistsIfNeededAsync(int? parentId)
    {
        if (parentId == null) return;

        var existingParent = await _repository.GetByIdAsync(parentId.Value);
        if (existingParent == null)
        {
            throw new NotFoundException(nameof(Category) + $" not found with parentId: {parentId.Value}");
        }
    }
}
