using DapperSandboxApp.Entities;

namespace DapperSandboxApp.Repositories;

internal interface IBlogRepository
{
    internal Task<Blog?> GetByIdAsync( int id );
}
