using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Orchestrator.Like.Orchestrator;

public class LikeOrchestrator(
    ILikeRepository repository) : ILikeOrchestrator
{
    public async Task<LikeDto> GetLikeByIdAsync(int likeId)
    {
        return await repository.GetLikeByIdAsync(likeId)
               ?? throw new NotFoundException(
                   $"Like with id {likeId} was not found");
    }

    public async Task<LikeDto> CreateLikeAsync(int postId, int userId)
    {
        if (await IsPostLikedByUserAsync(postId, userId))
            throw new InvalidArgumentException(
                $"Post with id {postId} is already liked by user with id {userId}");
        return await repository.CreateLikeAsync(postId, userId);
    }

    public async Task<LikeDto> RemoveLikeAsync(int postId, int userId)
    {
        return await repository.RemoveLikeAsync(postId, userId)
               ?? throw new NotFoundException(
                   $"Like for post {postId} by user {userId} was not found");
    }

    public async Task<int> CountByPostIdAsync(int postId)
    {
        return await repository.CountByPostIdAsync(postId);
    }

    public async Task<bool> IsPostLikedByUserAsync(int postId, int userId)
    {
        return await repository.IsPostLikedByUserAsync(postId, userId);
    }

    public async Task<List<LikeDto>> GetLikesOnPostAsync(int postId)
    {
        return await repository.GetLikesOnPostAsync(postId);
    }

    public async Task<List<LikeDto>> GetAllLikesFromUserAsync(int userId)
    {
        return await repository.GetAllLikesFromUserAsync(userId);
    }
}
