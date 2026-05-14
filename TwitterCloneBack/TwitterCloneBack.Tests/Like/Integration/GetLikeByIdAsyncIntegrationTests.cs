using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Tests.Like.Integration;

public class GetLikeByIdAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory<Program> factory)
    : LikeIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetByIdAsync_WhenLikeExists_ReturnsLike()
    {
        // Arrange
        int id = 1;
        await SeedAsync(
            [CreateUser(1)],
            [CreatePost(1, 1)],
            [CreateLike(id, 1, 1)]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var like = await response.Content.ReadFromJsonAsync<LikeDto>();

        Assert.NotNull(like);
        Assert.Equal(id, like.Id);
        Assert.Equal(1, like.PostId);
        Assert.Equal(1, like.LikedById);
    }

    [Fact]
    public async Task GetByIdAsync_WhenLikeDoesntExist_ReturnsNotFound()
    {
        // Arrange
        await SeedAsync([], [], []);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
