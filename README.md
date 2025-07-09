# 🧠 Smart Cache Management System

A high-performance backend system that reduces redundant API calls through intelligent caching and change detection mechanisms. Built using **ASP.NET Core Web API**, **ADO.NET**, **Redis**, and **Docker**, this system is ideal for mobile applications requiring minimal data synchronization and efficient resource usage.

---

## 📦 Features

✅ **Change Detection**: Timestamp-based cache validation to detect updates in services, stories, and categories.  
✅ **Generic ADO.NET Repository**: Reusable CRUD logic using reflection with table-based configuration.  
✅ **Stored Procedure Integration**: Safe and efficient insertions using SQL Server stored procedures with parameter binding (no SQL injection risk).  
✅ **Redis Caching**: Cache-aside strategy with TTL, change tracking, and invalidation.  
✅ **GenericEntityCacheService**: Entity-agnostic caching logic using factory-based DI.  
✅ **Custom Category Tree Logic**: Recursive child category retrieval and circular reference protection.  
✅ **Mobile Simulation Client**: Simulates smart client behavior, only requesting changed data.  
✅ **Dockerized Deployment**: Runs Redis, SQL Server, and Web API seamlessly via Docker Compose.

---

## 🧱 Architecture Overview

```
Mobile Client ↔️ ChangeCheckController ↔️ Redis ↔️ ADO.NET Repositories ↔️ SQL Server
```

### 🔄 Data Flow:

1. Client sends timestamps to check for changes.
2. Server compares with Redis stored timestamps.
3. If changes exist, relevant data is fetched via ADO.NET and returned.
4. Data is cached with a fresh LastModified value.

---

## 🔧 Setup Instructions

### Prerequisites:
- .NET 8 SDK
- Docker & Docker Compose
- Redis CLI (optional for manual inspection)

### 1. Clone Repository
```bash
git clone https://github.com/Kamilm55/SmartCacheAPI.git
```

### 2. Run with Docker
```bash
docker-compose up --build
```

> ⚠️ **Note**: Before running, make sure to apply EF Core migrations once.

---

## 🧪 Change Detection Logic

### Example Client Request
```json
{
  "CategoryTimestamp": "2024-07-01T12:00:00Z",
  "ServiceTimestamp": null,
  "StoryTimestamp": "2024-07-05T08:00:00Z"
}
```

### Redis Timestamps
```js
categories:lastmodified => 2024-07-09T12:32:10Z
services:lastmodified   => null
stories:lastmodified    => 2024-07-08T10:20:45Z
```

> Server compares Redis timestamps with incoming ones and returns only updated data.

---

## 🧠 Core Components

### ✅ Generic ADO.NET Repository

A flexible repository using C# reflection to auto-map entity properties to SQL columns:

```csharp
public abstract class GenericRepository<T> where T : class, new()
```

#### Features:
- Dynamic `SELECT`, `INSERT`, `UPDATE`, and `DELETE` operations
- Auto-maps SqlDataReader → Entity using **reflection**
- Supports nullable types, strings, DateTime, and primitives
- Cleanly injectable into derived repositories

#### Example:
```csharp
public class ServicesRepository : GenericRepository<Service>, IServicesRepository
{
    public ServicesRepository(IConfiguration config) : base(config, "Services") {}
}
```

### 🔐 SQL Injection Protection

Every query uses **parameterized statements** like:

```csharp
command.Parameters.AddWithValue("@Name", category.Name);
```

✅ This ensures that untrusted user input is never directly concatenated into SQL strings, which is the primary way SQL injection attacks succeed.

🔍 Why It Works:
* When using AddWithValue() or similar parameterized methods, the query is compiled with placeholders (@Name) before values are inserted.
* The input is passed as data, not executable code — so even if the user tries to inject SQL (e.g., '; DROP TABLE Categories; --), it will be treated as a literal string, not a command.
* This separates query logic from input, making it immune to most common injection attempts.

💡 Compared to:
```csharp
// ❌ Dangerous: vulnerable to SQL injection
var query = $"INSERT INTO Categories (Name) VALUES ('{userInput}')";
```
Using parameterized queries is the safest and recommended practice for interacting with SQL Server, especially when building dynamic systems like a generic ADO.NET repository.

---

### ✅ Stored Procedure Integration

EF Core migration adds the stored procedure:

```sql
CREATE PROCEDURE sp_CreateCategories
    @Name NVARCHAR(100),
    @IsActive BIT = 0,
    @ParentId INT = NULL,
    @Id INT OUTPUT
```

Executed using:

```csharp
var cmd = new SqlCommand("sp_CreateCategories") {
    CommandType = CommandType.StoredProcedure
};
cmd.Parameters.AddWithValue("@Name", category.Name);
// ...
```

✅ Prevents SQL injection  
✅ Clean business logic separation  
✅ Supports output parameters

---

### ✅ GenericEntityCacheService<T>

Reusable Redis caching service per entity type.

#### Main Features:
- `GetOrSetListEntityCacheAsync`: Caches entire table
- `GetOrSetSingleEntityCacheAsync`: Caches single item by ID
- `UpdateCacheAfterCreateAsync`: Updates list + single cache
- `InvalidateCacheAsync`: Invalidates both data + LastModified keys
- `GetLastModifiedAsync`: Fetches timestamp used for change detection

---

### ✅ Factory Pattern for Clean DI

Dynamically generates a generic caching service for each entity:

```csharp
_commonCacheService = cacheServiceFactory.Create<Category>(
    CacheKeys.CATEGORIES_DATA,
    CacheKeys.CATEGORIES_LASTMODIFIED
);
```

> Keeps services decoupled and clean.

---

## 📎 Final Notes

This system demonstrates:
- Clean architecture
- Efficient caching
- Safe database interaction
- Good coding practices
- Real-world production readiness

✅ Ideal for large-scale mobile backends that require real-time sync without constant polling.