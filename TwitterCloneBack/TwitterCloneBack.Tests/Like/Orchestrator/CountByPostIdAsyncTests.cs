using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class CountByPostIdAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task CountByPostIdAsync_ReturnsLikeCount()
    {
        // Arrange
        int postId = 1;
        int count = 3;

        _repositoryMock
            .Setup(r => r.CountByPostIdAsync(postId))
            .ReturnsAsync(count);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.CountByPostIdAsync(postId);

        // Assert
        Assert.Equal(count, result);
    }
}
