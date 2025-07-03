using System.ComponentModel.DataAnnotations;

namespace SmartCacheManagementSystem.Common.DTOs.Requests.Story;

public class StoryUpdateRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Content is required.")]
    [MinLength(10, ErrorMessage = "Content must be at least 10 characters long.")]
    public string Content { get; set; } = null!;

    public bool IsPublished { get; set; } = false;

   // [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
    public string? ImageUrl { get; set; }
}