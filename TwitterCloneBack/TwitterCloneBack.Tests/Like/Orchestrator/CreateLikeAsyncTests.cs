using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class CreateLikeAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateLikeAsync_WhenPostIsNotLiked_CreatesLike()
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
            .Setup(r => r.IsPostLikedByUserAsync(postId, userId))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(r => r.CreateLikeAsync(postId, userId))
            .ReturnsAsync(like);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.CreateLikeAsync(postId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(like, result);
    }

    [Fact]
    public async Task CreateLikeAsync_WhenPostIsAlreadyLiked_ThrowsError()
    {
        // Arrange
        int postId = 1;
        int userId = 1;

        _repositoryMock
            .Setup(r => r.IsPostLikedByUserAsync(postId, userId))
            .ReturnsAsync(true);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidArgumentException>(async () =>
            await likeOrchestrator.CreateLikeAsync(postId, userId));

        _repositoryMock.Verify(
            r => r.CreateLikeAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Never);
    }
}
