# Commands


## Insert

``` csharp
// Request
Blog request = new() { Title = "Blog 3", Url = "http://blog3.com", IsActive = true };


// Query
string query =
"""
insert into Blogs( Title, Url, IsActive ) values ( @Title, @Url, @IsActive )

select cast( SCOPE_IDENTITY() as int );
""";


// Parameters
DynamicParameters parameters = new();
parameters.Add( "Title", request.Title, DbType.String, ParameterDirection.Input );
parameters.Add( "Url", request.Url, DbType.String, ParameterDirection.Input );
parameters.Add( "IsActive", request.IsActive, DbType.Boolean, ParameterDirection.Input );


// Command
using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    int newId = await dbConnection.QuerySingleAsync<int>( query, parameters );

    // MapToResponse/Model
    Blog createdBlog = new() { Id = newId, Title = request.Title, ... };

    Console.WriteLine( $"BlogId: {newBLogId}" );
}
```



## Update

``` csharp
// Request
int blogId = 3;
Blog request = new() { Title = "Blog 3 Updated", Url = "http://blog3.com", IsActive = false };


// Parameters
DynamicParameters parameters = new();
parameters.Add( "BlogId", blogId, DbType.Int32 );
parameters.Add( "Title", request.Title, DbType.String );
parameters.Add( "Url", request.Url, DbType.String );
parameters.Add( "IsActive", request.IsActive, DbType.Boolean );


// Query
string query =
    """
    update Blogs
    set
      Title = @Title,
      Url = @Url,
      IsActive = @IsActive
    where
      BlogId = @BlogId;
    """;


// Command
using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    int numberOfRowsAffected = await dbConnection.ExecuteAsync( query, parameters );

    Console.WriteLine( $"{numberOfRowsAffected}" );
}
```


## Delete

``` csharp
// Request
int blogId = 3;


// Query
string query =
    """
    delete from Blogs where BlogId = @BlogId;
    """;

// Command
using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    int numberOfRowsAffected = dbConnection.Execute( query, new { BlogId = blogId } );

    Console.WriteLine( $"{numberOfRowsAffected}" );
}
```
