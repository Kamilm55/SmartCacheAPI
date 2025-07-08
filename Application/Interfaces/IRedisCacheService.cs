using Microsoft.EntityFrameworkCore.Storage;
using SmartCacheManagementSystem.Domain.Entities;
using IDatabase = StackExchange.Redis.IDatabase;

namespace SmartCacheManagementSystem.Application.Interfaces;

public interface IRedisCacheService
{
    Task SetCacheAsync<T>(string key, T data);
    Task<T?> GetCacheAsync<T>(string key);
    Task DeleteCacheAsync(string key);
    // Task SetChecksumAsync(string key, string checksum);
    // Task<string?> GetChecksumAsync(string key);
    Task SetCacheAsync(string categoriesData, int createdId, Category created);
    Task<T?> GetCacheAsync<T>(string categoriesData, int id);
    Task DeleteCacheAsync(string categoriesData, int existingId);
}