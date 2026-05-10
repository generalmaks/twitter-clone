using Moq;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;

namespace TwitterCloneBack.Tests.Like.Orchestrator;

public class GetLikesOnPostAsyncTests
{
    private readonly Mock<ILikeRepository> _repositoryMock = new();

    private LikeOrchestrator CreateLikeOrchestrator()
    {
        return new LikeOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetLikesOnPostAsync_WhenLikesExist_ReturnsLikes()
    {
        // Arrange
        int postId = 1;
        var likes = new List<LikeDto>
        {
            new()
            {
                Id = 1,
                PostId = postId,
                LikedById = 1
            },
            new()
            {
                Id = 2,
                PostId = postId,
                LikedById = 2
            }
        };

        _repositoryMock
            .Setup(r => r.GetLikesOnPostAsync(postId))
            .ReturnsAsync(likes);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.GetLikesOnPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(likes, result);
    }

    [Fact]
    public async Task GetLikesOnPostAsync_WhenNoLikesExist_ReturnsEmptyList()
    {
        // Arrange
        int postId = 1;

        _repositoryMock
            .Setup(r => r.GetLikesOnPostAsync(postId))
            .ReturnsAsync([]);

        var likeOrchestrator = CreateLikeOrchestrator();

        // Act
        var result = await likeOrchestrator.GetLikesOnPostAsync(postId);

        // Assert
        Assert.Empty(result);
    }
}
