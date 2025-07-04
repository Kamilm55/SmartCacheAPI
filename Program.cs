using Microsoft.EntityFrameworkCore;
using SmartCacheManagementSystem.Application.Interfaces;
using SmartCacheManagementSystem.Application.Services;
using SmartCacheManagementSystem.Extensions;
using SmartCacheManagementSystem.Infrastructure.Configuration;
using SmartCacheManagementSystem.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<IRedisCacheService,RedisCacheService>();

// --- Service Registrations ---
builder.Services.AddApplicationServices();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Middleware pipeline ---
app.UseGlobalMiddlewares();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
