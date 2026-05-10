using Moq;
using TwitterCloneBack.Model.Post.Contracts;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;

namespace TwitterCloneBack.Tests.Post.Orchestrator;

public class PatchPostAsyncTests
{
    private readonly Mock<IPostRepository> _repositoryMock = new();

    private PostOrchestrator CreatePostOrchestrator()
    {
        return new PostOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task PatchPostAsync_WhenPostExists_UpdatesProvidedFields()
    {
        // Arrange
        var existingPost = CreateExistingPost();
        var updatePost = new UpdatePost
        {
            Id = existingPost.Id,
            TextContent = "updated"
        };

        PostDto? capturedPost = null;

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(updatePost.Id))
            .ReturnsAsync(existingPost);

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(It.IsAny<PostDto>()))
            .Callback<PostDto>(post => capturedPost = post)
            .ReturnsAsync((PostDto post) => post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        await postOrchestrator.PatchPostAsync(updatePost);

        // Assert
        Assert.NotNull(capturedPost);
        Assert.Equal(updatePost.TextContent, capturedPost.TextContent);
    }

    [Fact]
    public async Task PatchPostAsync_WhenFieldsAreNull_KeepsExistingValues()
    {
        // Arrange
        var existingPost = CreateExistingPost();
        var updatePost = new UpdatePost
        {
            Id = existingPost.Id
        };

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(updatePost.Id))
            .ReturnsAsync(existingPost);

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(It.IsAny<PostDto>()))
            .ReturnsAsync((PostDto post) => post);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.PatchPostAsync(updatePost);

        // Assert
        Assert.Equal("existing", result.TextContent);
    }

    [Fact]
    public async Task PatchPostAsync_WhenPostDoesntExist_ThrowsError()
    {
        // Arrange
        var updatePost = new UpdatePost
        {
            Id = 1,
            TextContent = "updated"
        };

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(updatePost.Id))
            .ReturnsAsync((PostDto)null!);

        var postOrchestrator = CreatePostOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await postOrchestrator.PatchPostAsync(updatePost));

        _repositoryMock.Verify(
            r => r.UpdatePostAsync(It.IsAny<PostDto>()),
            Times.Never);
    }

    [Fact]
    public async Task PatchPostAsync_WhenRepositoryReturnsPost_ReturnsUpdatedPost()
    {
        // Arrange
        var existingPost = CreateExistingPost();
        var updatePost = new UpdatePost
        {
            Id = existingPost.Id,
            TextContent = "updated"
        };

        var updatedPost = new PostDto
        {
            Id = existingPost.Id,
            AuthorId = existingPost.AuthorId,
            TextContent = updatePost.TextContent
        };

        _repositoryMock
            .Setup(r => r.GetPostByIdAsync(updatePost.Id))
            .ReturnsAsync(existingPost);

        _repositoryMock
            .Setup(r => r.UpdatePostAsync(It.IsAny<PostDto>()))
            .ReturnsAsync(updatedPost);

        var postOrchestrator = CreatePostOrchestrator();

        // Act
        var result = await postOrchestrator.PatchPostAsync(updatePost);

        // Assert
        Assert.Equivalent(updatedPost, result);
    }

    private static PostDto CreateExistingPost()
    {
        return new PostDto
        {
            Id = 1,
            AuthorId = 1,
            TextContent = "existing"
        };
    }
}
