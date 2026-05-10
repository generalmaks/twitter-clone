using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Tests.Like.Integration;

public class GetLikesAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : LikeIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task GetLikesOnPostAsync_WhenLikesExist_ReturnsLikes()
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
        var response = await Http.GetAsync($"{ApiPrefix}/post/1");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var likes = await response.Content.ReadFromJsonAsync<List<LikeDto>>();

        Assert.NotNull(likes);
        Assert.Equal(2, likes.Count);
        Assert.Contains(likes, l => l.Id == 1 && l.LikedById == 1);
        Assert.Contains(likes, l => l.Id == 2 && l.LikedById == 2);
    }

    [Fact]
    public async Task GetAllLikesFromUserAsync_WhenLikesExist_ReturnsLikes()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1)],
            [CreatePost(1, 1), CreatePost(2, 1)],
            [
                CreateLike(1, 1, 1),
                CreateLike(2, 2, 1)
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/user/1");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var likes = await response.Content.ReadFromJsonAsync<List<LikeDto>>();

        Assert.NotNull(likes);
        Assert.Equal(2, likes.Count);
        Assert.Contains(likes, l => l.PostId == 1);
        Assert.Contains(likes, l => l.PostId == 2);
    }
}
