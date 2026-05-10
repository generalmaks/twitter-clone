using Moq;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class GetRepliesToPostAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetRepliesToPostAsync_WhenPostExists_ReturnsReplies()
    {
        // Arrange
        int id = 1;
        var replies = new List<PostDto>
        {
            new()
            {
                Id = 2,
                AuthorId = 1,
                ReplyToPostId = id,
                TextContent = "reply1"
            },
            new()
            {
                Id = 3,
                AuthorId = 1,
                ReplyToPostId = id,
                TextContent = "reply2"
            }
        };

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync(new PostDto
            {
                Id = id
            });

        _repositoryMock
            .Setup(r => r.GetRepliesToPostAsync(id))
            .ReturnsAsync(replies);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.GetRepliesToPostAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(replies, result);
    }

    [Fact]
    public async Task GetRepliesToPostAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(id))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await postOrchestrator.GetRepliesToPostAsync(id));

        _repositoryMock.Verify(
            r => r.GetRepliesToPostAsync(It.IsAny<int>()),
            Times.Never);
    }
}
