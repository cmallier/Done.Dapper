# Setup


## SqlServer

Create a database

``` sql
create database SandboxDapper
go

use SandboxDapper;
go

create table Blogs
(
    BlogId int identity(1,1) primary key,
    Title nvarchar(250) not null,
    Url nvarchar(1000) not null,
    IsActive bit not null
)

create table Posts
(
    PostId int identity(1,1) primary key,
    BlogId int not null,
    Title nvarchar(250) not null,
    Content nvarchar(max) not null,
    CreatedOn datetime not null,
    PublishedOn datetime not null,

    Constraint [FK_Posts_Blogs] Foreign Key( [BlogId] ) References [dbo].[Blogs] ( [BlogId] )
)
```


Add data (Blogs)

``` sql
insert into Blogs ( Title, Url, IsActive )
values
( 'Blog 1', 'http://blog1.com', 1 ),
( 'Blog 2', 'http://blog2.com', 0 )
```


Add data (Posts)

``` sql
insert into Posts ( BlogId, Title, Content, CreatedOn, PublishedOn )
values
( 1, 'Post 1-1', 'Content 1', '2020-01-01', '2023-01-01' ),
( 1, 'Post 1-2', 'Content 2', '2020-01-02', '2023-01-02' ),
( 1, 'Post 1-3', 'Content 3', '2020-01-03', '2023-01-03' ),
( 2, 'Post 2-1', 'Content 4', '2020-01-04', '2023-01-04' ),
( 2, 'Post 2-2', 'Content 5', '2020-01-05', '2023-01-05' ),
( 2, 'Post 2-3', 'Content 6', '2020-01-06', '2023-01-06' )
```
