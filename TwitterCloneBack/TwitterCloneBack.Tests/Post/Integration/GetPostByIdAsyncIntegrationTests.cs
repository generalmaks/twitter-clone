using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Tests.Post.Integration;

public class GetPostByIdAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task GetPostByIdAsync_WhenPostExists_ReturnsPost()
    {
        // Arrange
        int id = 1;
        await SeedAsync(
            [CreateUser(1)],
            [CreatePost(id, 1, "test")]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var post = await response.Content.ReadFromJsonAsync<GetPost>();

        Assert.NotNull(post);
        Assert.Equal(id, post.Id);
        Assert.Equal(1, post.AuthorId);
        Assert.Equal("test", post.TextContent);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenPostDoesntExist_ReturnsNotFound()
    {
        // Arrange
        int id = 1;
        await SeedAsync([], []);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
