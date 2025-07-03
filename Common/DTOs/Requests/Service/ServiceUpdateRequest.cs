using System.ComponentModel.DataAnnotations;

namespace SmartCacheManagementSystem.Common.DTOs.Requests;

public class ServiceUpdateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number.")]
    public float Price { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }
}