using Microsoft.EntityFrameworkCore;
using SmartCacheManagementSystem.Domain.Entities;

namespace SmartCacheManagementSystem.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; private set; }
    public DbSet<Service> Services { get; private set; }
    public DbSet<Story> Stories { get; private set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-Many (Self-referencing)
        // One child category ➝ can have only one (or no) parent category
        // One parent category ➝ can have many child categories
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict); // Don't allow deleting a parent if it has children.

    }
}
