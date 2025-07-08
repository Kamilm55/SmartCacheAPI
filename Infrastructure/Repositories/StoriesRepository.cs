using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class StoriesRepository : GenericRepository<Story>, IStoriesRepository,IGenericRepository<Story>
{
    
    public StoriesRepository(IConfiguration configuration) : base(configuration, "Stories")
    {
    } 
}