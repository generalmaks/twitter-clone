using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TwitterCloneBack.Dal.Like.Dao;
using TwitterCloneBack.Dal.User.Dao;

namespace TwitterCloneBack.Dal.Post.Dao;

public class PostDao
{
    [Key]
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int? ReplyToPostId { get; set; }

    [StringLength(200)]
    public string TextContent { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Posts")]
    public virtual UserDao Author { get; set; } = null!;

    [ForeignKey(nameof(ReplyToPostId))]
    [InverseProperty(nameof(Replies))]
    public virtual PostDao? ReplyToPost { get; set; }

    [InverseProperty(nameof(ReplyToPost))]
    public virtual ICollection<PostDao> Replies { get; set; } = [];

    public ICollection<LikeDao> Likes { get; set; } = new List<LikeDao>();
}