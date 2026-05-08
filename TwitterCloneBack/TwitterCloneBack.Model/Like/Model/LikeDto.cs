namespace TwitterCloneBack.Model.Like.Model;

public class LikeDto
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int LikedById { get; set; }

    public DateTime LikedAt { get; set; }
}