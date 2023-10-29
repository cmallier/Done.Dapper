namespace DapperSandboxApp.Entities;

internal class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? PublishedOn { get; set; }
}
