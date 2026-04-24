namespace TwitterCloneBack.Model.Model;

public class PostDto
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string TextContent { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }
}
