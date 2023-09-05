# Flatscha.EFCore.Api

[![Nuget](https://img.shields.io/nuget/v/Flatscha.EFCore.Api)](https://www.nuget.org/packages/Flatscha.EFCore.Api/)
![GitHub last commit](https://img.shields.io/github/last-commit/Flatscha/Flatscha.EFCore.Api)
![GitHub contributors](https://img.shields.io/github/contributors/Flatscha/Flatscha.EFCore.Api)


This is a package that generates all CRUD Minimal API endpoints for an Entity Framework context via source generation.

The following context was generated using `Scaffold-DbContext` from the package `Microsoft.EntityFrameworkCore.Tools`.

```csharp
public partial class TestContext : DbContext 
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
}
```

### Map Context
You can add all endpoints for all entities in a context using only one command.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Test"));
});

var app = builder.Build();

var api = app.MapGroup("api");
api.MapTestContext();

app.Run();
```

| HTTP Method | Endpoint                |
|-------------|-------------------------|
|  GET        | `/api/User/`            |
|  GET        | `/api/User/{id}`        |
|  POST       | `/api/User/Save/`       |
|  PUT        | `/api/User/Update/{id}` |
|  DELETE     | `/api/User/Delete/{id}` |

### Map Entity
If you do not want to add API endpoints for all entities in your context, mapping-methods for every entity seperatly are also generated.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Test"));
});

var app = builder.Build();

var api = app.MapGroup("api");
api.MapUser();

app.Run();
```