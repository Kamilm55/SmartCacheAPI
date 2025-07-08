namespace SmartCacheManagementSystem.Domain.Entities;
using System;

public class Story
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime? LastModified { get; set; }
    public bool IsPublished { get; set; }
    
    public string? ImageUrl { get; set; } // Optional
}
