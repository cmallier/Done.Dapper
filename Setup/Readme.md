# Dapper - Setup


## Packages

- Dapper
- Microsoft.Data.SqlClient


``` xml
<ItemGroup>
    <PackageReference Include="Dapper" Version="2.1.15" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.2" />
</ItemGroup>
```



## Create a DbContext

``` csharp
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyApp.Infrastructure.Persistence;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext( IConfiguration configuration )
    {
        _connectionString = configuration.GetConnectionString( "MyApp" )!;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection( _connectionString );
    }
}
```


## Register

``` csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence( this IServiceCollection services, IConfiguration configuration )
    {
        services.AddSingleton<DbContext>();

        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddRepositories( this IServiceCollection services )
    {
        services.AddScoped<IModelRepository, ModelRepository>();
        //...

        return services;
    }
}
```


## Register Repositories

Inside MyApp.Application
- IModelRepository

``` csharp
public interface IModelRepository
{
    // Classic
    Task<Model?> GetByIdAsync( Guid id );
    Task<List<Model>> ListAsync();
    Task<List<Model>> ListByIdsAsync( List<Guid> ids );

    Task AddModelAsync( Model model );

    Task UpdateAsync( Model model );
    Task UpdateRangeAsync( List<Model> models );

    Task RemoveAsync( Model model );
    Task RemoveRangeAsync( List<Model> models );

    Task<bool> ExistsAsync( Guid id );

    // More Specialized
    Task<List<Model>> ListBySubscriptionIdAsync( Guid subscriptionId );
    Task<Model?> GetModelByUserIdAsync( Guid userId );
```

Inside MyApp.Infrastructure.Persistence
- ModelRepository


``` csharp
internal class ModelRepository : IModelRepository
{
    private readonly DbContext _dbContext;

    public BlogRepository( DbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public async Task<Model?> GetByIdAsync( int id )
    {
        var parameters = new
        {
            Id = id
        };

        string query =
            """
            -- declare @BlogId int = 1

            select
                 b.BlogId as Id
                ,b.Title
                ,b.Url
                ,b.IsActive
            from
                 dbo.Blogs b
            where
                b.BlogId = @BlogId
            """;

        using( IDbConnection dbConnection = _dbContext.CreateConnection() )
        {
            Model? model = await dbConnection.QueryFirstOrDefaultAsync<Model>( query, parameters );

            return model;
        }
    }
}
```



For examples,

see
- [Cookbook/CopyAndPaste/Queries.md](../Cookbook/CopyAndPaste/Queries.md)
- [Cookbook/CopyAndPaste/Commands.md](../Cookbook/CopyAndPaste/Commands.md)
