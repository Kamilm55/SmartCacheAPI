using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(IConfiguration configuration) : base(configuration, "Categories")
    {
    }
    public Task<List<Category>> GetChildrenAsync(int parentId)
    {
        throw new NotImplementedException();
    }
    
    /*
     *
     * public async Task<Category?> GetCategoryWithFullChildrenTreeAsync(int id)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);
            if (category == null) return null;

            category.Children = await GetChildrenRecursiveAsync(id);
            return category;
        }

private async Task<List<Category>> GetChildrenRecursiveAsync(int parentId)
{
    var children = await _categoriesRepository.GetChildrenAsync(parentId);

// If there are children, the method calls itself recursively for each child’s Id to populate that child's Children collection.
// If there are no children (i.e., GetChildrenAsync(parentId) returns an empty list), the recursion stops because the foreach loop has no elements to iterate over — it does not call itself further.
    
    foreach (var child in children)
    {
        child.Children = await GetChildrenRecursiveAsync(child.Id); // give child.Id as parentId to find child's child
    }

    return children;
}

     */

}