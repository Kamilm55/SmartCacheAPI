using Microsoft.Data.SqlClient;
using SmartCacheManagementSystem.Domain.Entities;
using SmartCacheManagementSystem.Infrastructure.Repositories.Interfaces;

namespace SmartCacheManagementSystem.Infrastructure.Repositories;

public class StoriesRepository : GenericRepository<Story>, IStoriesRepository
{
    
    public StoriesRepository(IConfiguration configuration) : base(configuration, "Stories")
    {
    }
    /*private readonly string _connectionString;

    public StoriesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<Story>> GetAllAsync()
    {
        var stories = new List<Story>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("SELECT * FROM Stories", connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            stories.Add(new Story
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
                IsPublished = reader.GetBoolean(reader.GetOrdinal("IsPublished"))
            });
        }

        return stories;
    }

    public async Task<Story?> GetByIdAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("SELECT * FROM Stories WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;

        return new Story
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Title = reader.GetString(reader.GetOrdinal("Title")),
            Content = reader.GetString(reader.GetOrdinal("Content")),
            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
            LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
            IsPublished = reader.GetBoolean(reader.GetOrdinal("IsPublished"))
        };
    }

    public async Task<Story> CreateAsync(Story story)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(@"
            INSERT INTO Stories (Title, Content, ImageUrl, LastModified, IsPublished)
            OUTPUT INSERTED.Id
            VALUES (@title, @content, @imageUrl, @lastModified, @isPublished)", connection);

        command.Parameters.AddWithValue("@title", story.Title);
        command.Parameters.AddWithValue("@content", story.Content);
        command.Parameters.AddWithValue("@imageUrl", (object?)story.ImageUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@lastModified", story.LastModified);
        command.Parameters.AddWithValue("@isPublished", story.IsPublished);

        await connection.OpenAsync();

        var newId = (int)await command.ExecuteScalarAsync();
        story.Id = newId;

        return story;
    }

    public async Task<Story?> UpdateAsync(Story story)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(@"
            UPDATE Stories
            SET Title = @title,
                Content = @content,
                ImageUrl = @imageUrl,
                LastModified = @lastModified,
                IsPublished = @isPublished
            OUTPUT inserted.*
            WHERE Id = @id", connection);

        command.Parameters.AddWithValue("@id", story.Id);
        command.Parameters.AddWithValue("@title", story.Title);
        command.Parameters.AddWithValue("@content", story.Content);
        command.Parameters.AddWithValue("@imageUrl", (object?)story.ImageUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@lastModified", story.LastModified);
        command.Parameters.AddWithValue("@isPublished", story.IsPublished);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return null;

        return new Story
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Title = reader.GetString(reader.GetOrdinal("Title")),
            Content = reader.GetString(reader.GetOrdinal("Content")),
            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
            LastModified = reader.GetDateTime(reader.GetOrdinal("LastModified")),
            IsPublished = reader.GetBoolean(reader.GetOrdinal("IsPublished"))
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("DELETE FROM Stories WHERE Id = @id", connection);
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() > 0;
    }*/
    
}