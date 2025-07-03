namespace SmartCacheManagementSystem.Domain.Entities;
using System;

public class Service
{
    public int Id { get; set; }                    
    public string Name { get; set; } = null!;
    public float Price { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsActive { get; set; } = true;
    
    public string? Description { get; set; } // Optional
}
