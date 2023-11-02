# Queries


## Simple Select

```csharp
string sql =
    """
    select
        ...
    from
        dbo.Blogs b
    where
        b.Id = @Id
    """;

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    TModel? model = await dbConnection.QueryFirstOrDefaultAsync<TModel>( sql, param: new { Id = 1 } );

    return model;
}
```


## One-One

Expemple: Une personne a une seule Adresse

| PersonId | Name  | Street      | City     |
|----------|-------|-------------|----------|
| 1        | Anton | 1 main road | Montreal |
| 2        | Ben   | 678 Elm     | Quebec   |
| 3        | Carl  | 99 Keaton   | Ottawa   |


```csharp
string sql =
    """
    select
        p.PersonId,
        p.Name,
        a.Street,          -- SplitOn (Address)
        a.City
    from
        Persons p
        inner join Address a on p.PersonId = a.PersonId
    """;

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    IEnumerable<Person> people = dbConnection.Query<Person, Address, Person>(
        sql,
        ( person, address ) =>
        {
            person.Address = address;
            return person;
        },
        splitOn: "Street" );

    return people;
}
```


## Many -> many

Many Blog has one or many Posts

| BlogId | Name         | PostId | Title   |
|--------|--------------|--------|---------|
| 1      | Alimentation | 12     | Avocat  |
| 1      | Alimentation | 67     | Banane  |
| 1      | Alimentation | 345    | Carotte |
| 2      | Vehicule     | 23     | Auto    |
| 2      | Vehicule     | 3567   | Bateau  |
| 3      | Plante       | 249    | Cactus  |


```csharp
string sql =
    """
    select
        b.BlogId,         -- SplitOn (Blog)
        ...
        p.PostId,          -- SplitOn (Posts)
        ...
    from
        dbo.Blogs b
        inner join dbo.Posts p on p.BlogId = b.BlogId
    """;

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    IEnumerable<Blog> blogs = dbConnection.Query<Blog, Post, Blog>(
        sql,
        ( blog, post ) =>
        {
            blog.Posts.Add( post );
            return blog;
        },
        splitOn: "PostId" );


    ICollection<Blog> result = (blogs.GroupBy( b => b.BlogId ).Select( g =>
    {
        Blog groupedBlog = g.First();
        groupedBlog.Posts = g.Select( b => b.Posts.Single() ).ToList();

        return groupedBlog;
    } )).ToList();

    return result;
}
```


## Many -> many (V2)

```csharp
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
where
    b.BlogId = @Id
""";

// Dictionary outisde the Query
Dictionary<int, Blog> dictionary = new();

using( IDbConnection dbConnection = new SqlConnection( connectionString ) )
{
    IEnumerable<Blog> blogs = dbConnection.Query<Blog, Post, Blog>(
        sql,
        ( blog, post ) =>
        {
            if( dictionary.TryGetValue( blog.Id, out Blog? existingBlog ) )
            {
                blog = existingBlog
            }
            else
            {
                dictionary.Add( blog.Id, blog );
            }

            blog.Posts.Add( post );'

            return null;
        },
        param: new { Id = 1 },
        splitOn: "PostId" );
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
