using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Tests.Post.Integration;

public class GetPostsAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory<Program> factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetPostsAsync_WhenPostsExist_ReturnsPosts()
    {
        // Arrange
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(1, 1, "test1"),
                CreatePost(2, 1, "test2")
            ]);

        // Act
        var response = await Http.GetAsync(ApiPrefix);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
        Assert.Contains(posts, p => p.Id == 1 && p.TextContent == "test1");
        Assert.Contains(posts, p => p.Id == 2 && p.TextContent == "test2");
    }

    [Fact]
    public async Task GetPostsAsync_WhenNoPostsExist_ReturnsEmptyList()
    {
        // Arrange
        await SeedAsync([], []);

        // Act
        var response = await Http.GetAsync(ApiPrefix);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }
}
