using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class GetPostsAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPostsAsync_WhenPostsExist_ReturnsPosts()
    {
        // Arrange
        int page = 1;
        int pageSize = 2;
        var posts = new List<PostDto>
        {
            new()
            {
                Id = 1,
                AuthorId = 1,
                TextContent = "test1"
            },
            new()
            {
                Id = 2,
                AuthorId = 1,
                TextContent = "test2"
            }
        };

        _repositoryMock
            .Setup(r => r.GetPostsAsync(page, pageSize))
            .ReturnsAsync(posts);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetPostsAsync(page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(posts, result);
    }

    [Fact]
    public async Task GetPostsAsync_WhenNoPostsExist_ReturnsEmptyList()
    {
        // Arrange
        int page = 1;
        int pageSize = 20;
        var posts = new List<PostDto>();

        _repositoryMock
            .Setup(r => r.GetPostsAsync(page, pageSize))
            .ReturnsAsync(posts);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetPostsAsync(page, pageSize);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPostsAsync_ForwardsPaginationToRepository()
    {
        // Arrange
        int page = 3;
        int pageSize = 10;

        _repositoryMock
            .Setup(r => r.GetPostsAsync(page, pageSize))
            .ReturnsAsync([]);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.GetPostsAsync(page, pageSize);

        // Assert
        _repositoryMock.Verify(
            r => r.GetPostsAsync(page, pageSize),
            Times.Once);
    }
}
