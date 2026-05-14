using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Dal.Like.Dao;
using TwitterCloneBack.Dal.Post.Dao;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.Login;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Tests.Like.Integration;

public abstract class LikeIntegrationTestBase
{
    protected const string ApiPrefix = "/api/v1/like";
    protected readonly TwitterCloneWebApplicationFactory<Program> Factory;
    protected readonly HttpClient Http;

    protected LikeIntegrationTestBase(TwitterCloneWebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Http = factory.CreateClient();
    }

    protected void AuthorizeUser(int id)
    {
        using var scope = Factory.Services.CreateScope();
        var tokenGenerator =
            scope.ServiceProvider.GetRequiredService<JwtTokenGenerator>();

        var token = tokenGenerator.GenerateJwt(new UserDto
        {
            Id = id,
            Email = $"test{id}@test.com"
        });

        Http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected async Task SeedAsync(
        IEnumerable<UserDao> users,
        IEnumerable<PostDao> posts,
        IEnumerable<LikeDao> likes
    )
    {
        using var scope = Factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        db.Likes.RemoveRange(db.Likes);
        db.Posts.RemoveRange(db.Posts);
        db.Users.RemoveRange(db.Users);
        await db.SaveChangesAsync();

        db.Users.AddRange(users);
        db.Posts.AddRange(posts);
        db.Likes.AddRange(likes);
        await db.SaveChangesAsync();
    }

    protected static UserDao CreateUser(int id)
    {
        return new UserDao
        {
            Id = id,
            Username = $"test{id}",
            Email = $"test{id}@test.com",
            DisplayUsername = $"Test User {id}",
            PasswordHash = "hash"u8.ToArray(),
            CreatedAt = DateTime.UtcNow
        };
    }

    protected static PostDao CreatePost(int id, int authorId)
    {
        return new PostDao
        {
            Id = id,
            AuthorId = authorId,
            TextContent = $"post {id}",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    protected static LikeDao CreateLike(int id, int postId, int userId)
    {
        return new LikeDao
        {
            Id = id,
            PostId = postId,
            LikedById = userId,
            LikedAt = DateTime.UtcNow
        };
    }
}
