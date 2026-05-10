using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class GetAllLikesFromUserAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllLikesFromUserAsync_WhenLikesExist_ReturnsLikes()
    {
        // Arrange
        int userId = 1;
        var likes = new List<LikeDto>
        {
            new()
            {
                Id = 1,
                PostId = 1,
                LikedById = userId
            },
            new()
            {
                Id = 2,
                PostId = 2,
                LikedById = userId
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllLikesFromUserAsync(userId))
            .ReturnsAsync(likes);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.GetAllLikesFromUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(likes, result);
    }

    [Fact]
    public async Task GetAllLikesFromUserAsync_WhenNoLikesExist_ReturnsEmptyList()
    {
        // Arrange
        int userId = 1;

        _repositoryMock
            .Setup(r => r.GetAllLikesFromUserAsync(userId))
            .ReturnsAsync([]);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.GetAllLikesFromUserAsync(userId);

        // Assert
        Assert.Empty(result);
    }
}
