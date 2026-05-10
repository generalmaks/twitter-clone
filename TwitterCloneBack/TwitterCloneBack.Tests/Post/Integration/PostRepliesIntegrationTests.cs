using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Tests.Post.Integration;

public class PostRepliesIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task CountRepliesAsync_WhenPostExists_ReturnsReplyCount()
    {
        // Arrange
        int postId = 1;
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(postId, 1, "parent"),
                CreatePost(2, 1, "reply1", postId),
                CreatePost(3, 1, "reply2", postId)
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/count/{postId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);
        Assert.Equal(2, await response.Content.ReadFromJsonAsync<int>());
    }

    [Fact]
    public async Task GetRepliesToPostAsync_WhenPostExists_ReturnsReplies()
    {
        // Arrange
        int postId = 1;
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(postId, 1, "parent"),
                CreatePost(2, 1, "reply1", postId),
                CreatePost(3, 1, "reply2", postId)
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/replies/{postId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var replies = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(replies);
        Assert.Equal(2, replies.Count);
        Assert.Contains(replies, p => p.Id == 2 && p.TextContent == "reply1");
        Assert.Contains(replies, p => p.Id == 3 && p.TextContent == "reply2");
    }
}
