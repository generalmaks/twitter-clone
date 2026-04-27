namespace TwitterCloneBack.Api.Post.Contracts;

public class GetPost
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string TextContent { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
