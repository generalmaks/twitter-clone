using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class RemoveLikeAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task RemoveLikeAsync_WhenLikeExists_ReturnsRemovedLike()
    {
        // Arrange
        int postId = 1;
        int userId = 1;
        var like = new LikeDto
        {
            Id = 1,
            PostId = postId,
            LikedById = userId
        };

        _repositoryMock
            .Setup(r => r.RemoveLikeAsync(postId, userId))
            .ReturnsAsync(like);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.RemoveLikeAsync(postId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(like, result);
    }

    [Fact]
    public async Task RemoveLikeAsync_WhenLikeDoesntExist_ThrowsError()
    {
        // Arrange
        int postId = 1;
        int userId = 1;

        _repositoryMock
            .Setup(r => r.RemoveLikeAsync(postId, userId))
            .ReturnsAsync((LikeDto)null!);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await likeOrchestrator.RemoveLikeAsync(postId, userId));
    }
}
