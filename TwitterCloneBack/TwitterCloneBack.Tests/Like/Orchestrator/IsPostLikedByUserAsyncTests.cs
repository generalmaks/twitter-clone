using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class IsPostLikedByUserAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task IsPostLikedByUserAsync_WhenPostIsLiked_ReturnsTrue()
    {
        // Arrange
        int postId = 1;
        int userId = 1;

        _repositoryMock
            .Setup(r => r.IsPostLikedByUserAsync(postId, userId))
            .ReturnsAsync(true);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result =
            await likeOrchestrator.IsPostLikedByUserAsync(postId, userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsPostLikedByUserAsync_WhenPostIsNotLiked_ReturnsFalse()
    {
        // Arrange
        int postId = 1;
        int userId = 1;

        _repositoryMock
            .Setup(r => r.IsPostLikedByUserAsync(postId, userId))
            .ReturnsAsync(false);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result =
            await likeOrchestrator.IsPostLikedByUserAsync(postId, userId);

        // Assert
        Assert.False(result);
    }
}
