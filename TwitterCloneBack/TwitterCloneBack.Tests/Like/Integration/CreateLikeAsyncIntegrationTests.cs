using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Tests.Like.Integration;

public class CreateLikeAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : LikeIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task CreateLikeAsync_WhenUserIsAuthorized_CreatesLike()
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
        var response = await Http.PostAsync($"{ApiPrefix}/{postId}", null);
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

        Assert.Contains(
            db.Likes,
            l => l.PostId == postId && l.LikedById == userId);
    }

    [Fact]
    public async Task CreateLikeAsync_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Act
        var response = await Http.PostAsync($"{ApiPrefix}/1", null);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateLikeAsync_WhenPostIsAlreadyLiked_ReturnsBadRequest()
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
        var response = await Http.PostAsync($"{ApiPrefix}/{postId}", null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
