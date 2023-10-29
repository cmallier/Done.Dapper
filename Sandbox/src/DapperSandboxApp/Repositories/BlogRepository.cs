using System.Data;
using Dapper;
using DapperSandboxApp.Entities;
using DapperSandboxApp.Helpers;

namespace DapperSandboxApp.Repositories;

internal class BlogRepository : IBlogRepository
{
    private readonly DbContext _dbContext;

    public BlogRepository( DbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public async Task<Blog?> GetByIdAsync( int id )
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
            Blog? blog = await dbConnection.QueryFirstOrDefaultAsync<Blog>( query, parameters );

            return blog;
        }
    }
}
