using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Tests.Like.Integration;

public class RemoveLikeAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : LikeIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task RemoveLikeAsync_WhenUserIsAuthorized_RemovesLike()
    {
        // Arrange
        int userId = 1;
        int postId = 1;
        await SeedAsync(
            [CreateUser(userId)],
            [CreatePost(postId, userId)],
            [CreateLike(1, postId, userId)]);
        AuthorizeUser(userId);

        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/{postId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var like = await response.Content.ReadFromJsonAsync<LikeDto>();

        Assert.NotNull(like);
        Assert.Equal(postId, like.PostId);
        Assert.Equal(userId, like.LikedById);

        using var scope = Factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        Assert.DoesNotContain(
            db.Likes,
            l => l.PostId == postId && l.LikedById == userId);
    }

    [Fact]
    public async Task RemoveLikeAsync_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveLikeAsync_WhenLikeDoesntExist_ReturnsNotFound()
    {
        // Arrange
        int userId = 1;
        int postId = 1;
        await SeedAsync(
            [CreateUser(userId)],
            [CreatePost(postId, userId)],
            []);
        AuthorizeUser(userId);

        // Act
        var response = await Http.DeleteAsync($"{ApiPrefix}/{postId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
