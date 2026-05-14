using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Tests.Post.Integration;

public class DeletePostAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory<Program> factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory<Program>>
{
    [Fact]
    public async Task DeletePostAsync_WhenUserIsAuthor_DeletesPost()
    {
        // Arrange
        int authorId = 1;
        int postId = 1;
        await SeedAsync(
            [CreateUser(authorId)],
            [CreatePost(postId, authorId, "test")]);
        AuthorizeUser(authorId);

        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/{postId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var post = await response.Content.ReadFromJsonAsync<PostDto>();

        Assert.NotNull(post);
        Assert.Equal(postId, post.Id);
        Assert.True(post.IsDeleted);

        using var scope = Factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();
        var deletedPost = await db.Posts.FindAsync(postId);

        Assert.NotNull(deletedPost);
        Assert.True(deletedPost.IsDeleted);
    }

    [Fact]
    public async Task DeletePostAsync_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeletePostAsync_WhenUserIsNotAuthor_ReturnsForbidden()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1), CreateUser(2)],
            [CreatePost(1, 1, "test")]);
        AuthorizeUser(2);

        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/1");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
