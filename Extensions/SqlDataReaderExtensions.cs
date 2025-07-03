using Microsoft.Data.SqlClient;

namespace SmartCacheManagementSystem.Extensions;

public static class SqlDataReaderExtensions
{
    /// <summary>
    /// Checks if the given column exists in the SqlDataReader result set.
    /// </summary>
    public static bool HasColumn(this SqlDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}