using Microsoft.EntityFrameworkCore.Storage;
using IDatabase = StackExchange.Redis.IDatabase;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface IRedisCacheService
{
    IDatabase GetDatabase();
}