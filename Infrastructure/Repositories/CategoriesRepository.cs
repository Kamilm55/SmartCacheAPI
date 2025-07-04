using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(IConfiguration configuration) : base(configuration, "Categories")
    {
    }
    
    // Get all children tree --> even children of children
    public async Task<List<Category>> GetChildrenTreeAsync(int parentId, HashSet<int>? visited = null)
    {
        visited ??= new HashSet<int>();

        // Prevent cycles
        if (visited.Contains(parentId))
            return new List<Category>();

        visited.Add(parentId);

        var children = await GetFirstLevelChildrenAsync(parentId);

        foreach (var child in children)
        {
            // Recursively get the tree for each child
            child.Children = await GetChildrenTreeAsync(child.Id, visited);
        }

        return children;
    }

    private async Task<List<Category>> GetFirstLevelChildrenAsync(int parentId)
    {
        var categories = new List<Category>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand($"SELECT Id, Name, ParentId, LastModified, IsActive FROM {_tableName} WHERE ParentId = @parentId", connection);
        command.Parameters.AddWithValue("@parentId", parentId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (reader == null) throw new ArgumentNullException();
        
        while (await reader.ReadAsync())
        {
            var category = new Category
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) 
                    ? null 
                    : reader.GetInt32(reader.GetOrdinal("ParentId")),
                LastModified = reader.IsDBNull(reader.GetOrdinal("LastModified"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("LastModified")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            
                // Do not load Parent --> load externally if need
               // Parent = null
            };

            categories.Add(category);
        }

        return categories;
    }
    
    // todo: write this and create method as Function in migration up
    public async Task<bool> HasChildrenAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("SELECT TOP 1 1 FROM Categories WHERE ParentId = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();

        return result != null;
    }

}