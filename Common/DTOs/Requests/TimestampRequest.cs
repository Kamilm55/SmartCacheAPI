using Swashbuckle.AspNetCore.Annotations;

public class TimestampRequest
{
    [SwaggerSchema(Nullable = true, Description = "Category last modified timestamp")]
    public DateTime? CategoryTimestamp { get; set; } = null;
    
    [SwaggerSchema(Nullable = true, Description = "Story last modified timestamp")]
    public DateTime? StoryTimestamp { get; set; } = null;
    
    [SwaggerSchema(Nullable = true, Description = "Service last modified timestamp")]
    public DateTime? ServiceTimestamp { get; set; } = null;
}