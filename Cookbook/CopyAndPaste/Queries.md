# Queries


## Simple Select

```csharp
using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    string sql =
        """
        select
          ..
        from
          dbo.Blogs b
        where
          b.Id = @Id
        """;

    TModel? model = await dbConnection.QueryFirstOrDefaultAsync<TModel>( sql, param: new { Id = 1 } );

    return model;
}
```


## One-to-many

1 Blog has many Posts

- TModel = Blog
- TCollectionModel = Post

```csharp
using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    string sql =
        """
        select
          b.BlogId as Id,    -- SplitOn (Blog)
          ...
          p.PostId,          -- SplitOn (Posts)
          ...
        from
          dbo.Blogs b
          inner join dbo.Posts p on p.BlogId = b.BlogId
        """;

    IEnumerable<Blog> blogs = dbConnection.Query<Blog, Post, Blog>(
        sql,
        ( blog, post ) =>
        {
            blog.Posts.Add( post );
            return blog;
        },
        splitOn: "Id, PostId" );


    ICollection<Blog> result = (blogs.GroupBy( b => b.Id ).Select( g =>
    {
        Blog groupedBlog = g.First();
        groupedBlog.Posts = g.Select( b => b.Posts.Single() ).ToList();
        return groupedBlog;
    } )).ToList();
}
```


## Multiple SQL Statements with a Single Query
``` csharp

// Query (x2)
 string query =
"""
select BlogId as Id, * from Blogs where BlogId = @Id;

select * from Posts where BlogId = @Id;
""";

int id = 1;

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    using( SqlMapper.GridReader reader = await dbConnection.QueryMultipleAsync( query, new { id } ) )
    {
        Blog? blog = await reader.ReadSingleOrDefaultAsync<Blog>();

        if( blog is not null )
        {
            blog.Posts = (await reader.ReadAsync<Post>()).ToList();
        }

        Console.WriteLine( $"Title: {blog?.Title} Posts: {blog?.Posts.Count}" );
    }
}
```




## Stored Procedure

```csharp
string storedProcedureName = "dbo.GetBlogs";

DynamicParameters parameters = new();
parameters.Add( "Id", 1, DbType.Int32, ParameterDirection.Input );


using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    IEnumerable<Blog> blogs = await dbConnection.QueryAsync<Blog>(
        storedProcedureName,
        parameters,
        commandType: CommandType.StoredProcedure );

    return blogs;
}
```
