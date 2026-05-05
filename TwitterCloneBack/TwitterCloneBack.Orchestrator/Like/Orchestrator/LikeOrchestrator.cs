using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Orchestrator.Like.Orchestrator;

public class LikeOrchestrator(ILikeRepository repository) : ILikeOrchestrator
{
    public async Task<LikeDto> GetLikeByIdAsync(int likeId) =>
        await repository.GetLikeByIdAsync(likeId)
        ?? throw new KeyNotFoundException($"Like with id {likeId} was not found");

    public async Task<LikeDto> CreateLikeAsync(int postId, int userId)
    {
        if (await IsPostLikedByUserAsync(postId, userId))
            throw new ArgumentException($"Post with id {postId} is already liked by user with id {userId}");
        return await repository.CreateLikeAsync(postId, userId);
    }

    public async Task<LikeDto> RemoveLikeAsync(int postId, int userId) =>
        await repository.RemoveLikeAsync(postId, userId)
        ?? throw new KeyNotFoundException($"Like for post {postId} by user {userId} was not found");

    public async Task<int> CountByPostIdAsync(int postId) =>
        await repository.CountByPostIdAsync(postId);

    public async Task<bool> IsPostLikedByUserAsync(int postId, int userId) =>
        await repository.IsPostLikedByUserAsync(postId, userId);

    public async Task<List<LikeDto>> GetLikesOnPostAsync(int postId) =>
        await repository.GetLikesOnPostAsync(postId);

    public async Task<List<LikeDto>> GetAllLikesFromUserAsync(int userId) =>
        await repository.GetAllLikesFromUserAsync(userId);
}
