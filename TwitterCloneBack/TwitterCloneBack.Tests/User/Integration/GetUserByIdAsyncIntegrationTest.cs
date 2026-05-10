using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.User.Contracts;

namespace TwitterCloneBack.Tests.User.Integration;

public class GetUserByIdAsyncIntegrationTest : IClassFixture<TwitterCloneWebApplicationFactory>
{
    private const string ApiPrefix = "/api/v1/users";
    private readonly HttpClient _http;
    private readonly TwitterCloneWebApplicationFactory _factory;

    public GetUserByIdAsyncIntegrationTest(TwitterCloneWebApplicationFactory factory)
    {
        _factory = factory;
        _http = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExist_ReturnsUser()
    {
        // Arrange
        int id = 1;

        using var scope = _factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        db.Users.Add(new UserDao
        {
            Id = id,
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User",
            Bio = "test bio",
            PasswordHash = "hash"u8.ToArray(),
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync();

        // Act
        var response =
            await _http.GetAsync($"{ApiPrefix}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            content);

        var user = await response.Content.ReadFromJsonAsync<GetUser>();

        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesntExist_ReturnsNotFound()
    {
        // Arrange
        int id = 1;

        using var scope = _factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        // Act
        var response =
            await _http.GetAsync($"{ApiPrefix}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}