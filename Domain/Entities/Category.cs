namespace SmartCacheManagementSystem.Domain.Entities;

using System;

public class Category
{
    public int Id { get; set; }               
    public string Name { get; set; } = null!;
    
    // For nested categories (parent-child)
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }

    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property for child categories
    public ICollection<Category> Children { get; set; } = new List<Category>();
}
