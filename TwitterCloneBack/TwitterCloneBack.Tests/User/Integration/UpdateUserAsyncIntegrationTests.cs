using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.Login;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.User.Contracts;

namespace TwitterCloneBack.Tests.User.Integration;

public class UpdateUserAsyncIntegrationTests
    : IClassFixture<TwitterCloneWebApplicationFactory<Program>>
{
    private const string ApiPrefix = "/api/v1/users";
    private readonly TwitterCloneWebApplicationFactory<Program> _factory;
    private readonly HttpClient _http;

    public UpdateUserAsyncIntegrationTests(
        TwitterCloneWebApplicationFactory<Program> factory
    )
    {
        _factory = factory;
        _http = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserIsAuthorized_UpdatesUser()
    {
        // Arrange
        int id = 1;
        await SeedUserAsync(CreateUser(id));
        AuthorizeUser(id);

        var updateUser = new UpdateUser
        {
            Id = id,
            Username = "updated",
            DisplayUsername = "Updated User",
            Bio = "updated bio"
        };

        // Act
        var response = await _http.PatchAsJsonAsync(ApiPrefix, updateUser);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK, content);

        var user = await response.Content.ReadFromJsonAsync<GetUser>();

        Assert.NotNull(user);
        Assert.Equal(id, user.Id);
        Assert.Equal(updateUser.Username, user.Username);
        Assert.Equal(updateUser.DisplayUsername, user.DisplayUsername);
        Assert.Equal(updateUser.Bio, user.Bio);

        using var scope = _factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();
        var updatedUser = await db.Users.FindAsync(id);

        Assert.NotNull(updatedUser);
        Assert.Equal(updateUser.Username, updatedUser.Username);
        Assert.Equal(updateUser.DisplayUsername, updatedUser.DisplayUsername);
        Assert.Equal(updateUser.Bio, updatedUser.Bio);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserIsNotAuthorized_ReturnsUnauthorized()
    {
        // Arrange
        var updateUser = new UpdateUser
        {
            Id = 1,
            Username = "updated"
        };

        // Act
        var response = await _http.PatchAsJsonAsync(ApiPrefix, updateUser);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private void AuthorizeUser(int id)
    {
        using var scope = _factory.Services.CreateScope();
        var tokenGenerator =
            scope.ServiceProvider.GetRequiredService<JwtTokenGenerator>();

        var token = tokenGenerator.GenerateJwt(new UserDto
        {
            Id = id,
            Email = "test@test.com"
        });

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task SeedUserAsync(UserDao user)
    {
        using var scope = _factory.Services.CreateScope();
        var db =
            scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

        db.Users.RemoveRange(db.Users);
        await db.SaveChangesAsync();

        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    private static UserDao CreateUser(int id)
    {
        return new UserDao
        {
            Id = id,
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User",
            Bio = "test bio",
            PasswordHash = "hash"u8.ToArray(),
            CreatedAt = DateTime.UtcNow
        };
    }
}
