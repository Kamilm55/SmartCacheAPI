using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Interfaces.Factory;
using SmartCacheManagementSystem.Application.Mappers;
using SmartCacheManagementSystem.Application.Mappers.Interfaces;
using SmartCacheManagementSystem.Application.Services;
using SmartCacheManagementSystem.Application.Services.Common;
using SmartCacheManagementSystem.Infrastructure.Repositories;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;
using StackExchange.Redis;

namespace SmartCacheManagementSystem.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IStoriesService,StoriesService>();
        services.AddScoped<ITimestampService, TimestampService>();
        
        services.AddScoped<IGenericEntityCacheServiceFactory, GenericEntityCacheServiceFactory>();

        
        //services.AddScoped(typeof(IGenericEntityCacheService<>), typeof(GenericEntityCacheService<>));

        // var redis = ConnectionMultiplexer.Connect("localhost"); 
        // services.AddSingleton<IConnectionMultiplexer>(redis);
       // services.AddSingleton<IRedisCacheService, RedisCacheService>();
        
        // Repos
        services.AddScoped<IServicesRepository,ServicesRepository>();
        services.AddScoped<ICategoriesRepository,CategoriesRepository>();
        services.AddScoped<IStoriesRepository, StoriesRepository>();

        // Mappers
        services.AddScoped<IStoryMapper,StoryMapper>();
        services.AddScoped<IServiceMapper, ServiceMapper>();
        services.AddScoped<ICategoryMapper, CategoryMapper>();
        
        return services;
    }
}