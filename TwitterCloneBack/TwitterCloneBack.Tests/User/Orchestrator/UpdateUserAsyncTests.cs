using System.Text;
using Moq;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class UpdateUserAsyncTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserExists_UpdatesProvidedFields()
    {
        // Arrange
        var existingUser = CreateExistingUser();
        var updateUser = new UpdateUser
        {
            Id = existingUser.Id,
            Username = "updated",
            DisplayUsername = "Updated User",
            Bio = "updated bio"
        };

        UserDto? capturedUser = null;

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync(existingUser);

        _repositoryMock
            .Setup(r => r.UpdateUserAsync(It.IsAny<UserDto>()))
            .Callback<UserDto>(user => capturedUser = user)
            .ReturnsAsync((UserDto user) => user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.UpdateUserAsync(updateUser);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.Equal(updateUser.Username, capturedUser.Username);
        Assert.Equal(updateUser.DisplayUsername, capturedUser.DisplayUsername);
        Assert.Equal(updateUser.Bio, capturedUser.Bio);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenFieldsAreNull_KeepsExistingValues()
    {
        // Arrange
        var existingUser = CreateExistingUser();
        var passwordHash = existingUser.PasswordHash.ToArray();
        var updateUser = new UpdateUser
        {
            Id = existingUser.Id
        };

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync(existingUser);

        _repositoryMock
            .Setup(r => r.UpdateUserAsync(It.IsAny<UserDto>()))
            .ReturnsAsync((UserDto user) => user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.UpdateUserAsync(updateUser);

        // Assert
        Assert.Equal("existing", result.Username);
        Assert.Equal("Existing User", result.DisplayUsername);
        Assert.Equal("existing bio", result.Bio);
        Assert.Equal(passwordHash, result.PasswordHash);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenPasswordProvided_HashesPassword()
    {
        // Arrange
        var existingUser = CreateExistingUser();
        var updateUser = new UpdateUser
        {
            Id = existingUser.Id,
            Password = "newpassword123"
        };

        UserDto? capturedUser = null;

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync(existingUser);

        _repositoryMock
            .Setup(r => r.UpdateUserAsync(It.IsAny<UserDto>()))
            .Callback<UserDto>(user => capturedUser = user)
            .ReturnsAsync((UserDto user) => user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.UpdateUserAsync(updateUser);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.NotEqual(
            updateUser.Password,
            Encoding.UTF8.GetString(capturedUser.PasswordHash));
        Assert.True(
            BCrypt.Net.BCrypt.Verify(
                updateUser.Password,
                Encoding.UTF8.GetString(capturedUser.PasswordHash)));
    }

    [Fact]
    public async Task UpdateUserAsync_WhenPasswordIsTooShort_ThrowsError()
    {
        // Arrange
        var existingUser = CreateExistingUser();
        var updateUser = new UpdateUser
        {
            Id = existingUser.Id,
            Password = "short"
        };

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync(existingUser);

        var userOrchestrator = CreateUserOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await userOrchestrator.UpdateUserAsync(updateUser));

        _repositoryMock.Verify(
            r => r.UpdateUserAsync(It.IsAny<UserDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserDoesntExist_ThrowsError()
    {
        // Arrange
        var updateUser = new UpdateUser
        {
            Id = 1,
            Username = "updated"
        };

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync((UserDto)null!);

        var userOrchestrator = CreateUserOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await userOrchestrator.UpdateUserAsync(updateUser));

        _repositoryMock.Verify(
            r => r.UpdateUserAsync(It.IsAny<UserDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenRepositoryReturnsUser_ReturnsUpdatedUser()
    {
        // Arrange
        var existingUser = CreateExistingUser();
        var updateUser = new UpdateUser
        {
            Id = existingUser.Id,
            Username = "updated"
        };

        var updatedUser = new UserDto
        {
            Id = existingUser.Id,
            Username = updateUser.Username,
            Email = existingUser.Email,
            DisplayUsername = existingUser.DisplayUsername,
            Bio = existingUser.Bio,
            PasswordHash = existingUser.PasswordHash
        };

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(updateUser.Id))
            .ReturnsAsync(existingUser);

        _repositoryMock
            .Setup(r => r.UpdateUserAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(updatedUser);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.UpdateUserAsync(updateUser);

        // Assert
        Assert.Equivalent(updatedUser, result);
    }

    private static UserDto CreateExistingUser()
    {
        return new UserDto
        {
            Id = 1,
            Username = "existing",
            Email = "existing@test.com",
            DisplayUsername = "Existing User",
            Bio = "existing bio",
            PasswordHash = Encoding.UTF8.GetBytes(
                BCrypt.Net.BCrypt.HashPassword("password123"))
        };
    }
}
