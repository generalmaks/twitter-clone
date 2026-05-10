using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.User.Contracts;

namespace TwitterCloneBack.Tests.User.Integration;

public class GetUsersAsyncIntegrationTests
    : IClassFixture<TwitterCloneWebApplicationFactory>
{
    private const string ApiPrefix = "/api/v1/users";
    private readonly TwitterCloneWebApplicationFactory _factory;
    private readonly HttpClient _http;

    public GetUsersAsyncIntegrationTests(
        TwitterCloneWebApplicationFactory factory
    )
    {
        _factory = factory;
        _http = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsersAsync_WhenUsersExist_ReturnsUsers()
    {
        // Arrange
        await SeedUsersAsync(
        [
            CreateUser(1, "test1"),
            CreateUser(2, "test2")
        ]);

        // Act
        var response = await _http.GetAsync(ApiPrefix);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var users = await response.Content.ReadFromJsonAsync<List<GetUser>>();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count);
        Assert.Equal(1, users[0].Id);
        Assert.Equal("test1", users[0].Username);
        Assert.Equal(2, users[1].Id);
        Assert.Equal("test2", users[1].Username);
    }

    [Fact]
    public async Task GetUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
    {
        // Arrange
        await SeedUsersAsync([]);

        // Act
        var response = await _http.GetAsync(ApiPrefix);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var users = await response.Content.ReadFromJsonAsync<List<GetUser>>();

        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(-1, 20)]
    [InlineData(1, -5)]
    [InlineData(0, 0)]
    [InlineData(-1, -1)]
    public async Task GetUsersAsync_InvalidArgumentsProvided_ReturnsBadRequest(
        int page,
        int pageSize
    )
    {
        // Act
        var response =
            await _http.GetAsync(
                $"{ApiPrefix}?page={page}&pageSize={pageSize}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task SeedUsersAsync(List<UserDao> users)
    {
        using var scope = _factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        db.Users.RemoveRange(db.Users);
        await db.SaveChangesAsync();

        db.Users.AddRange(users);
        await db.SaveChangesAsync();
    }

    private static UserDao CreateUser(int id, string username)
    {
        return new UserDao
        {
            Id = id,
            Username = username,
            Email = $"{username}@test.com",
            DisplayUsername = $"Test User {id}",
            Bio = $"test bio {id}",
            PasswordHash = "hash"u8.ToArray(),
            CreatedAt = DateTime.UtcNow
        };
    }
}
