using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class DeletePostAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeletePostAsync_WhenPostExists_ReturnsDeletedPost()
    {
        // Arrange
        int id = 1;
        var post = new PostDto
        {
            Id = id,
            AuthorId = 1,
            TextContent = "test",
            IsDeleted = true
        };

        _repositoryMock
            .Setup(r => r.DeletePostAsync(id))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.DeletePostAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(post, result);
    }

    [Fact]
    public async Task DeletePostAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.DeletePostAsync(id))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await postOrchestrator.DeletePostAsync(id));
    }

    [Fact]
    public async Task DeletePostAsync_ForwardsIdToRepository()
    {
        // Arrange
        int id = 1;
        var post = new PostDto
        {
            Id = id
        };

        _repositoryMock
            .Setup(r => r.DeletePostAsync(id))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.DeletePostAsync(id);

        // Assert
        _repositoryMock.Verify(
            r => r.DeletePostAsync(id),
            Times.Once);
    }
}
