using System.Data;
using Dapper;
using DapperSandboxApp.Entities;
using Microsoft.Data.SqlClient;

string connectionString = "Server=DESKTOP-HOME; Database=SandboxDapper; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True;";


#region Query (Simple)
//using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
//{
//    string sql =
//        """
//        select
//           b.BlogId as Id
//          ,b.Title
//          ,b.Url
//          ,b.IsActive
//        from
//          dbo.Blogs b
//        where
//          b.BlogId = @BlogId
//        """;

//    Blog? blog1 = await dbConnection.QueryFirstOrDefaultAsync<Blog>( sql, param: new { BlogId = 1 } );

//    Console.WriteLine( $"BlogId: {blog1.Id}, Title: {blog1.Title}, Url: {blog1.Url}, IsActive: {blog1.IsActive}" );
//}
#endregion


#region Query (one-to-many)
//using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
//{
//    string sql =
//        """
//        select
//          b.BlogId as Id,    -- SplitOn (Blog)
//          b.Title,
//          b.Url,
//          b.IsActive,
//          p.PostId,          -- SplitOn (Posts)
//          p.Title,
//          p.Content,
//          p.CreatedOn,
//          p.PublishedOn
//        from
//          dbo.Blogs b
//          inner join dbo.Posts p on p.BlogId = b.BlogId
//        """;

//    IEnumerable<Blog> blogs = dbConnection.Query<Blog, Post, Blog>(
//        sql,
//        ( blog, post ) =>
//        {
//            blog.Posts.Add( post );
//            return blog;
//        },
//        splitOn: "Id, PostId" );


//    ICollection<Blog> result = (blogs.GroupBy( b => b.Id ).Select( g =>
//    {
//        Blog groupedBlog = g.First();
//        groupedBlog.Posts = g.Select( b => b.Posts.Single() ).ToList();
//        return groupedBlog;
//    } )).ToList();


//    Console.WriteLine( $"BlogId: {result.First().Id}, Title: {result.First().Title}, Posts: {result.First().Posts.Count}" );
//}
#endregion


#region Query (Multiple)
string query =
"""
select BlogId as Id, * from Blogs where BlogId = @Id;

select * from Posts where BlogId = @Id;
""";

int id = 1;

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    using SqlMapper.GridReader reader = await dbConnection.QueryMultipleAsync( query, new { id } );
    Blog? blog = await reader.ReadSingleOrDefaultAsync<Blog>();

    if( blog is not null )
    {
        blog.Posts = (await reader.ReadAsync<Post>()).ToList();
    }

    Console.WriteLine( $"Title: {blog?.Title} Posts: {blog?.Posts.Count}" );
}
#endregion


#region Command (Insert)
//using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
//{
//    Blog newBlog = new() { Title = "Blog 3", Url = "http://blog3.com", IsActive = true };

//    // Parameters
//    DynamicParameters parameters = new();
//    parameters.Add( "Title", newBlog.Title, DbType.String, ParameterDirection.Input );
//    parameters.Add( "Url", newBlog.Url, DbType.String, ParameterDirection.Input );
//    parameters.Add( "IsActive", newBlog.IsActive, DbType.Boolean, ParameterDirection.Input );

//    string sql =
//        """
//        insert into Blogs( Title, Url, IsActive ) values (@Title, @Url, @IsActive);

//        select cast( SCOPE_IDENTITY() as int );
//        """;

//    int newBLogId = dbConnection.QuerySingle<int>( sql, parameters );

//    Console.WriteLine( $"BlogId: {newBLogId}" );
//}
#endregion


#region Command (Update)
//using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
//{
//    int blogId = 3;
//    Blog request = new() { Title = "Blog 3 Updated", Url = "http://blog3.com", IsActive = false };

//    // Parameters
//    DynamicParameters parameters = new();
//    parameters.Add( "BlogId", blogId, DbType.Int32 );
//    parameters.Add( "Title", request.Title, DbType.String );
//    parameters.Add( "Url", request.Url, DbType.String );
//    parameters.Add( "IsActive", request.IsActive, DbType.Boolean );

//    string sql =
//        """
//        update Blogs
//        set Title = @Title,
//            Url = @Url,
//            IsActive = @IsActive
//        where BlogId = @BlogId;
//        """;

//    int numberOfRowsAffected = dbConnection.Execute( sql, parameters );

//    Console.WriteLine( $"{numberOfRowsAffected}" );
//}
#endregion


#region Command (Delete)
//using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
//{
//    int blogId = 3;

//    string query =
//        """
//        delete from Blogs where BlogId = @BlogId;
//        """;

//    int numberOfRowsAffected = dbConnection.Execute( query, new { BlogId = blogId } );

//    Console.WriteLine( $"{numberOfRowsAffected}" );
//}
#endregion


Console.WriteLine( "End" );
