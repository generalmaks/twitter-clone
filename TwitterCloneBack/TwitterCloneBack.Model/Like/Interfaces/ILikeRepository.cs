using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Model.Like.Interfaces;

public interface ILikeRepository
{
    Task<LikeDto> GetLikeByIdAsync(int likeId);
    Task<LikeDto> CreateLikeAsync(int postId, int userId);
    Task<LikeDto> RemoveLikeAsync(int postId, int userId);
    Task<int> CountByPostIdAsync(int postId);
    Task<bool> IsPostLikedByUserAsync(int postId, int userId);
    Task<List<LikeDto>> GetLikesOnPostAsync(int postId);
    Task<List<LikeDto>> GetAllLikesFromUserAsync(int userId);
}