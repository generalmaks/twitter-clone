using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class GetLikeByIdAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetLikeByIdAsync_WhenLikeExists_ReturnsLike()
    {
        // Arrange
        int id = 1;
        var like = new LikeDto
        {
            Id = id,
            PostId = 1,
            LikedById = 1
        };

        _repositoryMock
            .Setup(r => r.GetLikeByIdAsync(id))
            .ReturnsAsync(like);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.GetLikeByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(like, result);
    }

    [Fact]
    public async Task GetLikeByIdAsync_WhenLikeDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.GetLikeByIdAsync(id))
            .ReturnsAsync((LikeDto)null!);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await likeOrchestrator.GetLikeByIdAsync(id));
    }
}
