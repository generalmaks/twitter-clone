using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class GetPostByIdAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenPostExists_ReturnsPost()
    {
        // Arrange
        int id = 1;
        var post = new PostDto
        {
            Id = id,
            AuthorId = 1,
            TextContent = "test"
        };

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetPostByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(post, result);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await postOrchestrator.GetPostByIdAsync(id));
    }
}
