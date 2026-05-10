using System.Text;
using Moq;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class CreateUserAsyncTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_WhenUserIsValid_CreatesUserWithMappedFields()
    {
        // Arrange
        var createUser = new CreateUser
        {
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User",
            Bio = "test bio",
            Password = "password123"
        };

        UserDto? capturedUser = null;

        _repositoryMock
            .Setup(r => r.CreateUserAsync(It.IsAny<UserDto>()))
            .Callback<UserDto>(user => capturedUser = user)
            .ReturnsAsync((UserDto user) => user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.CreateUserAsync(createUser);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.Equal(createUser.Username, capturedUser.Username);
        Assert.Equal(createUser.Email, capturedUser.Email);
        Assert.Equal(createUser.DisplayUsername, capturedUser.DisplayUsername);
        Assert.Equal(createUser.Bio, capturedUser.Bio);
    }

    [Fact]
    public async Task CreateUserAsync_WhenUserIsValid_HashesPassword()
    {
        // Arrange
        var createUser = new CreateUser
        {
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User",
            Password = "password123"
        };

        UserDto? capturedUser = null;

        _repositoryMock
            .Setup(r => r.CreateUserAsync(It.IsAny<UserDto>()))
            .Callback<UserDto>(user => capturedUser = user)
            .ReturnsAsync((UserDto user) => user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.CreateUserAsync(createUser);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.NotNull(capturedUser.PasswordHash);
        Assert.NotEqual(
            createUser.Password,
            Encoding.UTF8.GetString(capturedUser.PasswordHash));
        Assert.True(
            BCrypt.Net.BCrypt.Verify(
                createUser.Password,
                Encoding.UTF8.GetString(capturedUser.PasswordHash)));
    }

    [Fact]
    public async Task CreateUserAsync_WhenRepositoryReturnsUser_ReturnsCreatedUser()
    {
        // Arrange
        var createUser = new CreateUser
        {
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User",
            Password = "password123"
        };

        var createdUser = new UserDto
        {
            Id = 1,
            Username = createUser.Username,
            Email = createUser.Email,
            DisplayUsername = createUser.DisplayUsername,
            PasswordHash = Encoding.UTF8.GetBytes("hash")
        };

        _repositoryMock
            .Setup(r => r.CreateUserAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(createdUser);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.CreateUserAsync(createUser);

        // Assert
        Assert.Equivalent(createdUser, result);
    }
}
