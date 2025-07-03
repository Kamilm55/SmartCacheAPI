using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Services;
using SmartCacheManagementSystem.Infrastructure.Repositories;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IStoriesService,StoriesService>();
        
        services.AddScoped<IServicesRepository,ServicesRepository>();
        services.AddScoped<ICategoriesRepository,CategoriesRepository>();
        services.AddScoped<IStoriesRepository, StoriesRepository>();


        return services;
    }
}