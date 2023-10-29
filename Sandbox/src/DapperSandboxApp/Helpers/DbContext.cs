using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperSandboxApp.Helpers;

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
