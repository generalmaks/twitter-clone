using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class CreatePostAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreatePostAsync_WhenPostIsValid_CreatesPost()
    {
        // Arrange
        var post = new PostDto
        {
            AuthorId = 1,
            TextContent = "test",
            ReplyToPostId = null
        };

        _repositoryMock
            .Setup(r => r.CreatePostAsync(post))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.CreatePostAsync(post);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(post, result);
    }

    [Fact]
    public async Task CreatePostAsync_ForwardsPostToRepository()
    {
        // Arrange
        var post = new PostDto
        {
            AuthorId = 1,
            TextContent = "test"
        };

        _repositoryMock
            .Setup(r => r.CreatePostAsync(post))
            .ReturnsAsync(post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.CreatePostAsync(post);

        // Assert
        _repositoryMock.Verify(
            r => r.CreatePostAsync(post),
            Times.Once);
    }
}
