
using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class ServicesRepository : GenericRepository<Service>,IServicesRepository
{
    public ServicesRepository(IConfiguration configuration) : base(configuration, "Services")
    {
    }
  /*private readonly string _connectionString;
 
     public ServicesRepository(IConfiguration configuration)
     {
         _connectionString = configuration.GetConnectionString("DefaultConnection")!;
     }
 
     public async Task<List<Service>> GetAllAsync()
     {
         var services = new List<Service>();

         await using var connection = new SqlConnection(_connectionString);
         await using var command = new SqlCommand("SELECT * FROM Services", connection);
 
         await connection.OpenAsync();
         await using var reader = await command.ExecuteReaderAsync();
 
         while (await reader.ReadAsync())
         {
             services.Add(new Service
             {
                 Id = reader.GetInt32(reader.GetOrdinal("Id")),
                 Name = reader.GetString(reader.GetOrdinal("Name")),
                 Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                 Price = (float)reader.GetDouble(reader.GetOrdinal("Price")),
                 LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
                 IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
             });
         }
 
         return services;
     }
     public async Task<Service?> GetByIdAsync(int id)
     {
         await using var connection = new SqlConnection(_connectionString);
         await using var command = new SqlCommand("SELECT * FROM Services WHERE Id = @id", connection);
         command.Parameters.AddWithValue("@id", id);
 
         await connection.OpenAsync();
         await using var reader = await command.ExecuteReaderAsync();
 
         if (!await reader.ReadAsync()) return null;
 
         return new Service
         {
             Id = reader.GetInt32(reader.GetOrdinal("Id")),
             Name = reader.GetString(reader.GetOrdinal("Name")),
             Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
             Price = (float)reader.GetDouble(reader.GetOrdinal("Price")),
             LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
             IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
         };
     }
     public async Task<Service> CreateAsync(Service service)
     {
         await using var connection = new SqlConnection(_connectionString);
         await using var command = new SqlCommand(@"
            INSERT INTO Services (Name, Description, Price, LastModified, IsActive)
            OUTPUT INSERTED.Id
            VALUES (@name, @desc, @price, @lastModified, @isActive)", connection);

         command.Parameters.AddWithValue("@name", service.Name);
         command.Parameters.AddWithValue("@desc", (object?)service.Description ?? DBNull.Value);
         command.Parameters.AddWithValue("@price", service.Price);
         command.Parameters.AddWithValue("@lastModified", service.LastModified);
         command.Parameters.AddWithValue("@isActive", service.IsActive);

         await connection.OpenAsync();

         var newId = (int)await command.ExecuteScalarAsync();
         service.Id = newId; // set the generated ID back to the entity

         return service;
     }
     public async Task<Service> UpdateAsync(Service service)
     {
         using var connection = new SqlConnection(_connectionString);
         using var command = new SqlCommand(@"
             UPDATE Services
             SET Name = @name,
                 Description = @desc,
                 Price = @price,
                 LastModified = @lastModified,
                 IsActive = @isActive
             WHERE Id = @id", connection);
 
         command.Parameters.AddWithValue("@id", service.Id);
         command.Parameters.AddWithValue("@name", service.Name);
         command.Parameters.AddWithValue("@desc", (object?)service.Description ?? DBNull.Value);
         command.Parameters.AddWithValue("@price", service.Price);
         command.Parameters.AddWithValue("@lastModified", service.LastModified);
         command.Parameters.AddWithValue("@isActive", service.IsActive);
 
         await connection.OpenAsync();
         await using var reader = await command.ExecuteReaderAsync();
         
         // Map returned updated row to Service entity
         var updatedService = new Service
         {
             Id = reader.GetInt32(reader.GetOrdinal("Id")),
             Name = reader.GetString(reader.GetOrdinal("Name")),
             Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
             Price = (float)reader.GetDouble(reader.GetOrdinal("Price")),
             LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
             IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
         };

         return updatedService;
     }
     public async Task<bool> DeleteAsync(int id)
     {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("DELETE FROM Services WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
 
         await connection.OpenAsync();
         return await command.ExecuteNonQueryAsync() > 0;
     }*/
  
}