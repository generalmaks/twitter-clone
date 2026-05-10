using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class CountRepliesAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task CountRepliesAsync_WhenPostExists_ReturnsReplyCount()
    {
        // Arrange
        int id = 1;
        int count = 3;

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync(new PostDto
            {
                Id = id
            });

        _repositoryMock
            .Setup(r => r.CountRepliesAsync(id))
            .ReturnsAsync(count);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.CountRepliesAsync(id);

        // Assert
        Assert.Equal(count, result);
    }

    [Fact]
    public async Task CountRepliesAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await postOrchestrator.CountRepliesAsync(id));

        _repositoryMock.Verify(
            r => r.CountRepliesAsync(It.IsAny<int>()),
            Times.Never);
    }
}
