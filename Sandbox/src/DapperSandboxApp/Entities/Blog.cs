namespace DapperSandboxApp.Entities;

internal class Blog
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Url { get; set; } = null!;

    public bool IsActive { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
