using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class GetPostsByTextSearchAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_WhenPostsMatchSearch_ReturnsPosts()
    {
        // Arrange
        const string search = "test";
        const int page = 1;
        const int pageSize = 2;
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
            .Setup(r => r.GetPostsByTextSearchAsync(search, page, pageSize))
            .ReturnsAsync(posts);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetPostsByTextSearchAsync(search, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(posts, result);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_WhenNoPostsMatchSearch_ReturnsEmptyList()
    {
        // Arrange
        const string search = "nothing";
        const int page = 1;
        const int pageSize = 20;
        var posts = new List<PostDto>();

        _repositoryMock
            .Setup(r => r.GetPostsByTextSearchAsync(search, page, pageSize))
            .ReturnsAsync(posts);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetPostsByTextSearchAsync(search, page, pageSize);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_ForwardsParametersToRepository()
    {
        // Arrange
        const string search = "query";
        const int page = 3;
        const int pageSize = 10;

        _repositoryMock
            .Setup(r => r.GetPostsByTextSearchAsync(search, page, pageSize))
            .ReturnsAsync([]);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.GetPostsByTextSearchAsync(search, page, pageSize);

        // Assert
        _repositoryMock.Verify(
            r => r.GetPostsByTextSearchAsync(search, page, pageSize),
            Times.Once);
    }
}
