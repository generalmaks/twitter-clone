using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Tests.Post.Integration;

public class CreatePostAsyncIntegrationTests(
    TwitterCloneWebApplicationFactory factory)
    : PostIntegrationTestBase(factory),
        IClassFixture<TwitterCloneWebApplicationFactory>
{
    [Fact]
    public async Task CreatePostAsync_WhenUserIsAuthorized_CreatesPost()
    {
        // Arrange
        int authorId = 1;
        await SeedAsync([CreateUser(authorId)], []);
        AuthorizeUser(authorId);

        var createPost = new CreatePost
        {
            TextContent = "created post"
        };

        // Act
        var response = await Http.PostAsJsonAsync(ApiPrefix, createPost);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var post = await response.Content.ReadFromJsonAsync<PostDto>();

        Assert.NotNull(post);
        Assert.Equal(authorId, post.AuthorId);
        Assert.Equal(createPost.TextContent, post.TextContent);
        Assert.False(post.IsDeleted);

        using var scope = Factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        Assert.Contains(
            db.Posts,
            p => p.AuthorId == authorId &&
                 p.TextContent == createPost.TextContent);
    }

    [Fact]
    public async Task CreatePostAsync_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Arrange
        var createPost = new CreatePost
        {
            TextContent = "created post"
        };

        // Act
        var response = await Http.PostAsJsonAsync(ApiPrefix, createPost);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /*
     Created separate variables because [new string('s', 257)] isn't compile-time
     constant and thus cant be put inside [InlineData()], and creating 257-long 
     string by hand is bad idea
    */
    public static IEnumerable<object?[]> InvalidUsernames =>
    [
        [""],
        [null],
        [new string('s', 257)]
    ];

    [Theory]
    [MemberData(nameof(InvalidUsernames))]
    public async Task CreatePostAsync_WhenTextContentIsRequired_ReturnsBadRequest(
        string? textContent
    )
    {
        // Arrange
        int authorId = 1;
        await SeedAsync([CreateUser(authorId)], []);
        AuthorizeUser(authorId);

        var createPost = new CreatePost
        {
            TextContent = textContent!
        };

        // Act
        var response = await Http.PostAsJsonAsync(ApiPrefix, createPost);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePostAsync_WhenTextContentIsTooLong_ReturnsBadRequest()
    {
        // Arrange
        int authorId = 1;
        await SeedAsync([CreateUser(authorId)], []);
        AuthorizeUser(authorId);

        var createPost = new CreatePost
        {
            TextContent = new string('a', 257)
        };

        // Act
        var response = await Http.PostAsJsonAsync(ApiPrefix, createPost);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
