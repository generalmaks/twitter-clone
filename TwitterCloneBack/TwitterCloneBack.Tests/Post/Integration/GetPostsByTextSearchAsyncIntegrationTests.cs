using System.Net;
using System.Net.Http.Json;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Tests.Post.Integration;

public class GetPostsByTextSearchAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory<Program> factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetPostsByTextSearchAsync_WhenPostsMatchSearch_ReturnsPosts()
    {
        // Arrange
        const string search = "match";
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(1, 1, "this is a match"),
                CreatePost(2, 1, "no m-a-t-c-h here"),
                CreatePost(3, 1, "another matching post")
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/search/{search}/page/1/pageSize/20");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
        Assert.Contains(posts, p => p.Id == 1);
        Assert.Contains(posts, p => p.Id == 3);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_WhenNoPostsMatchSearch_ReturnsEmptyList()
    {
        // Arrange
        const string search = "nothing";
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(1, 1, "first post"),
                CreatePost(2, 1, "second post")
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/search/{search}/page/1/pageSize/20");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_HandlesPagination()
    {
        // Arrange
        const string search = "match";
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(1, 1, "match 1"),
                CreatePost(2, 1, "match 2"),
                CreatePost(3, 1, "match 3")
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/search/{search}/page/2/pageSize/2");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Single(posts);
        // Based on OrderByDescending(p => p.CreatedAt) and SeedAsync adding minutes based on ID, 
        // ID 3 is newest, ID 2 is middle, ID 1 is oldest.
        // Page 1 (size 2): ID 3, ID 2
        // Page 2 (size 2): ID 1
        Assert.Equal(1, posts[0].Id);
    }

    [Fact]
    public async Task GetPostsByTextSearchAsync_DoesNotReturnDeletedPosts()
    {
        // Arrange
        const string search = "match";
        await SeedAsync(
            [CreateUser(1)],
            [
                CreatePost(1, 1, "match but deleted", isDeleted: true),
                CreatePost(2, 1, "match and not deleted")
            ]);

        // Act
        var response = await Http.GetAsync($"{ApiPrefix}/search/{search}/page/1/pageSize/20");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var posts = await response.Content.ReadFromJsonAsync<List<GetPost>>();

        Assert.NotNull(posts);
        Assert.Single(posts);
        Assert.Equal(2, posts[0].Id);
    }
}
