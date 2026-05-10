using System.Net;
using System.Net.Http.Json;

namespace TwitterCloneBack.Tests.Like.Integration;

public class LikeStatusIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : LikeIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task CountByPostIdAsync_WhenLikesExist_ReturnsCount()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1), CreateUser(2)],
            [CreatePost(1, 1)],
            [
                CreateLike(1, 1, 1),
                CreateLike(2, 1, 2)
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/post/1/count");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);
        Assert.Equal(2, await response.Content.ReadFromJsonAsync<int>());
    }

    [Fact]
    public async Task IsPostLikedByUserAsync_WhenPostIsLiked_ReturnsTrue()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1)],
            [CreatePost(1, 1)],
            [CreateLike(1, 1, 1)]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/post/1/user/1");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);
        Assert.True(await response.Content.ReadFromJsonAsync<bool>());
    }

    [Fact]
    public async Task IsPostLikedByUserAsync_WhenPostIsNotLiked_ReturnsFalse()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1)],
            [CreatePost(1, 1)],
            []);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/post/1/user/1");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);
        Assert.False(await response.Content.ReadFromJsonAsync<bool>());
    }
}
