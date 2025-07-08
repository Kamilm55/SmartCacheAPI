
using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class ServicesRepository : GenericRepository<Service>,IServicesRepository,IGenericRepository<Service>
{
    public ServicesRepository(IConfiguration configuration) : base(configuration, "Services")
    {
    }
  
}