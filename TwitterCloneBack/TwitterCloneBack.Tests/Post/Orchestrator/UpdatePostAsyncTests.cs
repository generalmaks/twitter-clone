using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class UpdatePostAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task UpdatePostAsync_WhenPostExists_ReturnsUpdatedPost()
    {
        // Arrange
        var post = new PostDto
        {
            Id = 1,
            AuthorId = 1,
            TextContent = "updated"
        };

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(post))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.UpdatePostAsync(post);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(post, result);
    }

    [Fact]
    public async Task UpdatePostAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        var post = new PostDto
        {
            Id = 1,
            AuthorId = 1,
            TextContent = "updated"
        };

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(post))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await postOrchestrator.UpdatePostAsync(post));
    }

    [Fact]
    public async Task UpdatePostAsync_ForwardsPostToRepository()
    {
        // Arrange
        var post = new PostDto
        {
            Id = 1,
            AuthorId = 1,
            TextContent = "updated"
        };

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(post))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.UpdatePostAsync(post);

        // Assert
        _repositoryMock.Verify(
            r => r.UpdatePostAsync(post),
            Times.Once);
    }
}
