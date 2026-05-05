using System.ComponentModel.DataAnnotations;

namespace TwitterCloneBack.Post.Contracts;

public class CreatePost
{
    public int? ReplyToPostId { get; set; }

    [StringLength(256, MinimumLength = 1)]
    public string TextContent { get; set; } = null!;
}