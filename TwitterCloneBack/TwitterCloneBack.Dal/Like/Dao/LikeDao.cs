using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.Post.Dao;
using TwitterCloneBack.Dal.User.Dao;

namespace TwitterCloneBack.Dal.Like.Dao;

[Index(nameof(PostId), nameof(LikedById), IsUnique = true)]
public class LikeDao
{
    [Key]
    public int Id { get; set; }
    
    public int PostId { get; set; }
    public int LikedById { get; set; }
    
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(PostId))]
    public PostDao Post { get; set; } = null!;
    [ForeignKey(nameof(LikedById))]
    public UserDao LikedBy { get; set; } = null!;
}