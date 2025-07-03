using SmartCacheManagementSystem.Extensions;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

public abstract class GenericRepository<T> where T : class, new()
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public GenericRepository(IConfiguration configuration, string tableName)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _tableName = tableName;
    }

    public async Task<List<T>> GetAllAsync()
    {
        var list = new List<T>();
        var props = GetMappableProperties();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand($"SELECT * FROM {_tableName}", connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var entity = new T();
            
            // If the value read from the database is not null, then convert it to the correct type of the property (handling nullable types if needed), and set that value on the object
            foreach (var prop in props) // iterate every prop of current entity
            {
                if (!reader.HasColumn(prop.Name)) continue; // extension method

                var value = reader[prop.Name]; // prop.Name acts like a key -> return corresponding value
                if (value != DBNull.Value)
                    prop.SetValue(entity, Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                    // SetValue sets the value of that property on the given object (entity).
            }
            list.Add(entity);
        }

        return list;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var props = GetMappableProperties();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand($"SELECT * FROM {_tableName} WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;

        var entity = new T();
        foreach (var prop in props)
        {
            if (!reader.HasColumn(prop.Name)) continue;

            var value = reader[prop.Name];
            if (value != DBNull.Value)
                prop.SetValue(entity, Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
        }

        return entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var propsOfCurrentEntity = GetMappableProperties().Where(p => p.Name != "Id").ToList(); // all props except Id
        var columns = string.Join(", ", propsOfCurrentEntity.Select(p => p.Name)); // Name, Surname
        var values = string.Join(", ", propsOfCurrentEntity.Select(p => "@" + p.Name));// @Name, @Surname

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(
            $"INSERT INTO {_tableName} ({columns}) OUTPUT INSERTED.Id VALUES ({values})", connection);

        foreach (var prop in propsOfCurrentEntity)
        {
            var value = prop.GetValue(entity) ?? DBNull.Value; // value of current entity field
            command.Parameters.AddWithValue("@" + prop.Name, value); // Set value into @fieldName
        }

        await connection.OpenAsync();
        var id = (int)await command.ExecuteScalarAsync();

        typeof(T).GetProperty("Id")?.SetValue(entity, id);
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        var props = GetMappableProperties().ToList(); // include Id
        var setClause = string.Join(", ", props.Where(p => p.Name != "Id").Select(p => $"{p.Name} = @{p.Name}")); // exclude Id

           // {setClause}-da Id olmamalidi amma Id = @Id burda olmalidi

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(
            $"UPDATE {_tableName} SET {setClause} OUTPUT INSERTED.* WHERE Id = @Id", connection);

        foreach (var prop in props)
        {
            var value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.AddWithValue("@" + prop.Name, value); // replace @fieldName with the actual value
        }

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        
        // if update  -> return entity 
        if (!await reader.ReadAsync()) return null;
        return entity;
        
        // var result = new T();
        // var allProps = GetMappableProperties();
        //
        // foreach (var prop in allProps)
        // {
        //     if (!reader.HasColumn(prop.Name)) continue;
        //
        //     var value = reader[prop.Name];
        //     if (value != DBNull.Value)
        //         prop.SetValue(result, Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
        // }

        //return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand($"DELETE FROM {_tableName} WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() > 0;
    }


    // returns a list of public instance properties
    private static List<PropertyInfo> GetMappableProperties()
    {
        return typeof(T) // Gets the type metadata for type T
            .GetProperties(BindingFlags.Public | BindingFlags.Instance) // Gets all public and instance-level properties (not static) of the type T
            .Where(p =>
                p.CanRead && //  Property has a getter.
                p.CanWrite && // setter
                (p.PropertyType.IsPrimitive ||
                 p.PropertyType == typeof(string) ||
                 p.PropertyType == typeof(DateTime) ||
                 Nullable.GetUnderlyingType(p.PropertyType) != null)
            )
            .ToList();
    }
}
